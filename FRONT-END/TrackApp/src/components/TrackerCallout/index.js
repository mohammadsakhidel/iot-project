import React from 'react';
import { StyleSheet, Text, View } from 'react-native';
import PropTypes from 'prop-types';
import * as vars from '../../styles/vars';

function TrackerCallout(props) {

    const {
        tracker,
        locationData
    } = props;

    return (
        <View style={styles.container}>
            <Text style={styles.displayName}>{tracker.displayName}</Text>

        </View>
    )
}

TrackerCallout.propTypes = {
    locationData: PropTypes.object,
    tracker: PropTypes.object
};

const styles = StyleSheet.create({
    container: {
        padding: vars.PAD_SMALL
    },
    displayName: {
        color: vars.COLOR_PRIMARY_D1,
        fontWeight: 'bold',
        textAlign: 'center'
    }
});

export default TrackerCallout;
