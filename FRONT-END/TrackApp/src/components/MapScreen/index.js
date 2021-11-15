import React, { Component, useEffect, useState, useRef } from 'react';
import { View, Dimensions, StyleSheet, Text, Image } from 'react-native';
import { Avatar } from 'react-native-elements';
import MapView, { Marker, Callout, AnimatedRegion } from 'react-native-maps';
import { connect } from 'react-redux';
import * as vars from '../../styles/vars';
import TrackerService from '../../api/services/tracker-service';
import { TouchableHighlight, TouchableOpacity } from 'react-native-gesture-handler';
import TrackerMarker from '../TrackerMarker';
import Icon from '../Icon';
import TrackerCallout from '../TrackerCallout';
import { getTrackerInfoAsync } from '../../utils/storage-util';
import { updateLocation } from '../../redux/actions';
import Location from '../../helpers/location';

const REGION_DELTA = 0.004;
const ANIM_DELAY = 300;

const MapScreen = (props) => {

    // State:

    const [selectedTracker, setSelectedTracker] = useState(null);
    const [mapRegion, setMapRegion] = useState(null);
    const [mapTouched, setMapTouched] = useState(false);

    // Props:

    const {
        locationUpdates,
        trackers,
        updateLocation,
        connections
    } = props;

    // Refs:

    const mapRef = useRef(null);
    const selectedMarkerRef = useRef(null);

    // Effects:

    // Set last location report from storage on component did mount:
    useEffect(() => {
        (async function () {
            trackers.forEach(async tracker => {
                if (!tracker.showOnMap)
                    return;

                const trackerInfo = await getTrackerInfoAsync(tracker.id);
                if (!trackerInfo || !trackerInfo.lastLocation)
                    return;

                if (!locationUpdates || !locationUpdates[tracker.id]) {
                    setTimeout(() => {
                        updateLocation(tracker.id, trackerInfo.lastLocation);
                    }, 200);
                }
            });
        })();
    }, []);

    // New Location Update Came Effect:
    useEffect(() => {
        if (locationUpdates) {

            // Fit to all supplied markers if not any selected:
            if (!selectedTracker) {
                if (!mapTouched) {
                    const markers = Object.keys(locationUpdates);
                    mapRef.current?.fitToSuppliedMarkers(markers, {
                        edgePadding: {
                            top: vars.PAD_DOUBLE,
                            left: vars.PAD_DOUBLE,
                            right: vars.PAD_DOUBLE,
                            bottom: vars.PAD_DOUBLE
                        }
                    });
                }
            }
            // Zoom on the selected tracker on update:
            else {
                const locData = locationUpdates[selectedTracker.id];
                if (locData) {
                    setMapRegion({
                        latitude: locData.latitude,
                        longitude: locData.longitude,
                        latitudeDelta: REGION_DELTA,
                        longitudeDelta: REGION_DELTA,
                    });
                    mapRef.current?.animateToRegion(mapRegion, ANIM_DELAY);
                }
            }
        }
    }, [locationUpdates]);

    // Map Region Changed Effect:
    useEffect(() => {
        if (mapRef.current) {
            if (mapRef.current && selectedMarkerRef.current) {
                mapRef.current.animateToRegion(mapRegion, ANIM_DELAY);
                setTimeout(() => {
                    selectedMarkerRef.current?.showCallout();
                }, ANIM_DELAY);
            }
        }
    }, [mapRegion]);

    // Event Handlers:

    const onItemPress = (tracker) => {

        if (!selectedTracker || selectedTracker.id != tracker.id) {
            const locData = locationUpdates[tracker.id];
            const region = locData ? {
                latitude: Number(locData.latitude),
                longitude: Number(locData.longitude),
                latitudeDelta: REGION_DELTA,
                longitudeDelta: REGION_DELTA
            } : null;
            setMapRegion(region);
            setSelectedTracker({ ...tracker });
        } else if (tracker.id == selectedTracker.id) {
            setSelectedTracker(null);
        }

    };

    const onMapTouch = () => {
        setMapTouched(true);
    };

    const onMarkerPress = (tracker) => {
        if (!selectedTracker || selectedTracker.id != tracker.id)
            setSelectedTracker(tracker);
    };

    // Functions:

    const isTrackerOnline = (trackerId, trackers, connections) => {

        if (!trackers || !connections)
            return false;

        const con = connections ? connections[trackerId] : null;
        const tracker = trackers ? trackers.find(t => t.id == trackerId) : null;
        const isonline = (con && con.status === "online") || (!con && tracker && tracker.status === "online");

        return isonline;
    };

    // Render:

    return (
        <View style={styles.container}>
            <MapView
                showsCompass={true}
                loadingEnabled={true}
                style={styles.map}
                ref={mapRef}
                maxZoomLevel={18}
                onTouchStart={onMapTouch}
            >
                {
                    Object.keys(locationUpdates).map(key => {
                        const data = locationUpdates[key];
                        const tracker = trackers.find((t) => t.id === key);
                        const isOnline = isTrackerOnline(tracker.id, trackers, connections);

                        return (
                            <Marker
                                key={key}
                                identifier={key}
                                coordinate={{ latitude: data.latitude, longitude: data.longitude }}
                                onPress={() => onMarkerPress(tracker)}
                                ref={(ref) => {
                                    if (selectedTracker && selectedTracker.id == key) {
                                        selectedMarkerRef.current = ref;
                                    }
                                }}
                            >
                                <TrackerMarker tracker={tracker} status={(isOnline ? 'online' : 'offline')} />
                                <Callout style={styles.callout}>
                                    <TrackerCallout
                                        tracker={tracker}
                                        locationData={data}
                                        status={(isOnline ? 'online' : 'offline')}
                                    />
                                </Callout>
                            </Marker>
                        );
                    })
                }
            </MapView>
            <View style={styles.bottomPanel}>
                <View style={styles.trackersContainer}>
                    {trackers.filter(t => t.showOnMap).map(tracker => {
                        return (
                            <View key={tracker.id}
                                style={(locationUpdates[tracker.id] ? styles.avatarEnabled : styles.avatarDisabled)}>

                                <TouchableOpacity onPress={() => onItemPress(tracker)}
                                    disabled={!locationUpdates[tracker.id]}>

                                    <Avatar
                                        rounded
                                        size="large"
                                        placeholderStyle={{ backgroundColor: vars.COLOR_GRAY_L2 }}
                                        source={{ uri: TrackerService.getIconUrl(tracker) }}
                                        containerStyle={(selectedTracker && selectedTracker.id == tracker.id ? styles.avatarSelected : styles.avatar)}
                                    />

                                </TouchableOpacity>

                            </View>
                        );
                    })}
                </View>
            </View>
        </View>
    );

};

const styles = StyleSheet.create({
    container: {
        flex: 1
    },
    map: {
        flex: 1,
        width: Dimensions.get('window').width,
    },
    bottomPanel: {
        backgroundColor: 'rgba(255,255,255,0.75)',
        paddingTop: vars.PAD_NORMAL,
        paddingStart: vars.PAD_NORMAL,
        //margin: vars.PAD_NORMAL,
        position: 'absolute',
        bottom: 0,
        left: 0,
        right: 0
    },
    avatar: {
        marginEnd: vars.PAD_NORMAL,
        marginBottom: vars.PAD_NORMAL,
        padding: vars.PAD_TINY,
        backgroundColor: vars.COLOR_GRAY_L3
    },
    avatarSelected: {
        marginEnd: vars.PAD_NORMAL,
        marginBottom: vars.PAD_NORMAL,
        padding: vars.PAD_SMALL,
        backgroundColor: vars.COLOR_PRIMARY_L1
    },
    avatarEnabled: {
        opacity: 1
    },
    avatarDisabled: {
        opacity: 0.2
    },
    trackersContainer: {
        flexDirection: 'row',
        flexWrap: 'wrap'
    },
    callout: {
        minWidth: 200
    }
});

const mapStateToProps = (state) => ({
    locationUpdates: state.locationUpdates,
    trackers: state.trackers,
    connections: state.connections
});

const mapDispatchToProps = (dispatch) => ({
    updateLocation: (trackerId, loc) => {

        const updateLocationEvent = {
            data: Location.toArray(loc),
            source: trackerId
        };

        const action = updateLocation(updateLocationEvent);
        dispatch(action);
    }
});

export default connect(mapStateToProps, mapDispatchToProps)(MapScreen);