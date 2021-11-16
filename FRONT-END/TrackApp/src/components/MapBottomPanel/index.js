import React from 'react';
import PropTypes from 'prop-types';
import { StyleSheet, View, TouchableOpacity } from 'react-native';
import * as vars from '../../styles/vars';
import TrackerService from '../../api/services/tracker-service';
import { Avatar } from 'react-native-elements';

function MapBottomPanel(props) {

    const {
        containerStyle,
        trackers,
        locationUpdates,
        selectedTracker, 
        onItemPress
    } = props;


    return (
        <View style={containerStyle}>
            <View style={styles.trackersContainer}>
                {trackers.filter(t => t.showOnMap).map(tracker => {
                    const isEnabled = locationUpdates[tracker.id];
                    const isSelected = selectedTracker && selectedTracker.id == tracker.id;
                    return (
                        <View key={tracker.id}
                            style={(isEnabled ? styles.avatarEnabled : styles.avatarDisabled)}>

                            <TouchableOpacity onPress={() => onItemPress(tracker)} disabled={!isEnabled}>

                                <Avatar
                                    rounded
                                    size="large"
                                    placeholderStyle={{ backgroundColor: vars.COLOR_GRAY_L2 }}
                                    source={{ uri: TrackerService.getIconUrl(tracker) }}
                                    containerStyle={(isSelected ? styles.avatarSelected : styles.avatar)}
                                />

                            </TouchableOpacity>

                        </View>
                    );
                })}
            </View>
        </View>
    )
}

const styles = StyleSheet.create({
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
    }
});

MapBottomPanel.propTypes = {
    containerStyle: PropTypes.object,
    trackers: PropTypes.array,
    locationUpdates: PropTypes.object,
    selectedTracker: PropTypes.object,
    onItemPress: PropTypes.func
};

export default MapBottomPanel;
