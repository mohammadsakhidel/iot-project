import React from 'react';
import { StyleSheet, Text, View } from 'react-native';
import PropTypes from 'prop-types';
import * as vars from '../../styles/vars';
import { Strings } from '../../i18n/strings';

function TrackerCallout(props) {

    const {
        tracker,
        locationData,
        status
    } = props;

    const isOnline = status == 'online';

    return (
        <View style={styles.container}>
            <Text style={styles.displayName}>
                {tracker.displayName}
            </Text>


            {/* Speed */}
            <View style={styles.row}>
                <View style={styles.rowTitleContainer}>
                    <Text style={styles.rowTitle}>
                        {Strings.Speed}
                    </Text>
                </View>
                <View style={styles.rowValueContainer}>
                    <Text style={styles.rowValue}>
                        {(isOnline ? locationData.speed : '-')}
                    </Text>
                </View>
            </View>

            {/* Connection */}
            <View style={styles.row}>
                <View style={styles.rowTitleContainer}>
                    <Text style={styles.rowTitle}>
                        {Strings.Connection}
                    </Text>
                </View>
                <View style={styles.rowValueContainer}>
                    <Text style={[styles.rowValue, (isOnline ? styles.online : styles.offline)]}>
                        {(isOnline ? Strings.Online : Strings.Offline)}
                    </Text>
                </View>
            </View>

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
        textAlign: 'center',
        paddingBottom: 6
    },
    row: {
        flexDirection: 'row',
        borderTopColor: vars.COLOR_GRAY_L3,
        borderTopWidth: 1
    },
    rowTitleContainer: {
        alignItems: 'flex-end',
        width: 80
    },
    rowValueContainer: {
        flexGrow: 1
    },
    rowTitle: {
        color: vars.COLOR_GRAY_L1,
        fontSize: vars.FS_BIT_SMALLER,
        padding: vars.PAD_SMALL
    },
    rowValue: {
        fontSize: vars.FS_BIT_SMALLER,
        padding: vars.PAD_SMALL
    },
    online: {
        color: vars.COLOR_SUCCESS
    },
    offline: {
        color: vars.COLOR_ERROR
    }
});

export default TrackerCallout;
