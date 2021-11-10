import React, { Component, useEffect, useState, useRef } from 'react';
import { View, Dimensions, StyleSheet, Text, Image } from 'react-native';
import { Avatar } from 'react-native-elements';
import MapView, { Marker, Callout } from 'react-native-maps';
import { connect } from 'react-redux';
import * as vars from '../../styles/vars';
import TrackerService from '../../api/services/tracker-service';
import { TouchableHighlight, TouchableOpacity } from 'react-native-gesture-handler';
import TrackerMarker from '../TrackerMarker';
import Icon from '../Icon';
import TrackerCallout from '../TrackerCallout';

const MapScreen = (props) => {

    // State:

    const [selectedItem, setSelectedItem] = useState(null);

    // Props:

    const {
        locationUpdates,
        trackers
    } = props;

    // Refs:

    const mapRef = useRef(null);
    const selectedMarkerRef = useRef(null);

    // Effects:

    // New Location Update Came Effect:
    useEffect(() => {
        if (mapRef.current) {
            const markers = Object.keys(locationUpdates);
            mapRef.current.fitToSuppliedMarkers(markers);
        }
    }, [locationUpdates]);

    // Selected Tracker Changed Effect:
    useEffect(() => {
        if (mapRef.current) {
            if (selectedItem) {
                mapRef.current.fitToSuppliedMarkers([selectedItem.id]);
                if (selectedMarkerRef.current) {
                    selectedMarkerRef.current.showCallout();
                }
            }
        }
    }, [selectedItem]);

    // Event Handlers:

    const onItemPress = (tracker) => {
        setSelectedItem({ ...tracker });
    };

    const onMarkerPress = (id) => {

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
            >
                {
                    Object.keys(locationUpdates).map(key => {
                        let data = locationUpdates[key];
                        let tracker = trackers.find((t) => t.id === key);

                        return (
                            <Marker
                                key={key}
                                identifier={key}
                                coordinate={{ latitude: data.latitude, longitude: data.longitude }}
                                onPress={() => onMarkerPress(key)}
                                ref={(ref) => {
                                    if (selectedItem && selectedItem.id == key) {
                                        selectedMarkerRef.current = ref;
                                    }
                                }}
                            >
                                <TrackerMarker tracker={tracker} />
                                <Callout style={styles.callout}>
                                    <TrackerCallout tracker={tracker} locationData={data} />
                                </Callout>
                            </Marker>
                        );
                    })
                }
            </MapView>
            <View style={styles.bottomPanel}>
                <View style={styles.trackersContainer}>
                    {trackers.map(tracker => {
                        return (
                            <TouchableOpacity key={tracker.id} onPress={() => onItemPress(tracker)}>
                                <Avatar
                                    rounded
                                    size="large"
                                    placeholderStyle={{ backgroundColor: vars.COLOR_GRAY_L2 }}
                                    source={{ uri: TrackerService.getIconUrl(tracker) }}
                                    containerStyle={(selectedItem && selectedItem.id == tracker.id ? styles.avatarSelected : styles.avatar)}
                                />
                            </TouchableOpacity>
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
        padding: vars.PAD_TINY,
        backgroundColor: vars.COLOR_PRIMARY_L1
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
    trackers: state.trackers
});

export default connect(mapStateToProps)(MapScreen);