import React, { Component, useEffect, useState } from 'react';
import { View, Dimensions, StyleSheet, Text } from 'react-native';
import { Avatar } from 'react-native-elements';
import MapView, { Marker } from 'react-native-maps';
import { connect } from 'react-redux';
import * as vars from '../../styles/vars';
import TrackerService from '../../api/services/tracker-service';
import { TouchableHighlight, TouchableOpacity } from 'react-native-gesture-handler';
import TrackerMarker from '../TrackerMarker';

const MapScreen = (props) => {

    // State:

    const [selectedItem, setSelectedItem] = useState(null);

    // Props:

    const {
        locationUpdates,
        trackers
    } = props;

    // Refs:

    const mapRef = React.createRef(null);

    // Effects:

    useEffect(() => {
        setTimeout(() => {

        }, 500);


        if (mapRef.current) {
            const markers = Object.keys(locationUpdates);
            if (!selectedItem) {
                mapRef.current.fitToSuppliedMarkers(markers);
            } else {
                console.log(selectedItem);
                mapRef.current.fitToSuppliedMarkers([selectedItem.id]);
            }
        }

    }, [locationUpdates, selectedItem]);

    // Event Handlers:

    const onItemPress = (tracker) => {
        setSelectedItem({ ...tracker });
    };

    // Render:

    return (
        <View style={styles.container}>
            <MapView
                style={styles.map}
                ref={mapRef}
                maxZoomLevel={15}
            >
                {
                    Object.keys(locationUpdates).map(key => {
                        let data = locationUpdates[key];
                        return (
                            <Marker
                                key={key}
                                identifier={key}
                                coordinate={{ latitude: data.latitude, longitude: data.longitude }}
                            >
                                <TrackerMarker tracker={trackers.find((t) => t.id === key)} />
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
                                    containerStyle={styles.avatar}
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
    trackersContainer: {
        flexDirection: 'row',
        flexWrap: 'wrap'
    }
});

const mapStateToProps = (state) => ({
    locationUpdates: state.locationUpdates,
    trackers: state.trackers
});

export default connect(mapStateToProps)(MapScreen);