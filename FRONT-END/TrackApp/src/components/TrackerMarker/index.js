import React from 'react'
import { View, Text, StyleSheet, ImageBackground, Image } from 'react-native';
import { Avatar } from 'react-native-elements';
import * as vars from '../../styles/vars';
import TrackerService from '../../api/services/tracker-service';

const marker = require('../../styles/images/marker.png');
const markerWidth = 55;
const markerHeight = 55;

function TrackerMarker(props) {

    const { tracker } = props;

    console.log(tracker);

    return (
        <View style={styles.markerContainer}>
            <Image
                style={styles.markerImage}
                resizeMode="contain"
                source={marker}
            />

            <Avatar
                rounded
                size={32}
                placeholderStyle={{ backgroundColor: vars.COLOR_GRAY_L2 }}
                source={{ uri: TrackerService.getIconUrl(tracker) }}
            />
        </View>
    )
}

const styles = StyleSheet.create({
    markerContainer: {
        width: markerWidth,
        height: markerHeight,
        alignItems: 'center',
        paddingTop: 3
    },
    markerImage: {
        position: 'absolute',
        left: 0,
        top: 0,
        width: markerWidth,
        height: markerHeight
    }
});

export default TrackerMarker;
