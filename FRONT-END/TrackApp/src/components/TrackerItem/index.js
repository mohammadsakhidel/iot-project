import React from 'react';
import { View, StyleSheet, ActivityIndicator } from 'react-native';
import Text from '../Text';
import * as vars from '../../styles/vars';
import { Card, Image } from 'react-native-elements';
import TrackerService from '../../api/services/tracker-service';
import SmallButton from '../SmallButton';
import { Strings } from '../../i18n/strings';

export default function TrackerItem(props) {

    const { item } = props;

    return (
        <View style={styles.container}>
            <Image
                source={{ uri: TrackerService.getIconUrl(item) }}
                style={styles.icon}
                PlaceholderContent={<ActivityIndicator />}
            />
            <View style={styles.textContainer}>
                <Text bold>{item.displayName}</Text>
                <View style={styles.actionsContainer}>
                    <SmallButton iconName="wrench" title={ Strings.Configure } />
                </View>
            </View>
        </View>
    );
}

const styles = StyleSheet.create({
    container: {
        padding: vars.PAD_BIT_MORE,
        borderBottomWidth: StyleSheet.hairlineWidth,
        borderBottomColor: vars.COLOR_GRAY_L3,
        flexDirection: 'row'
    },
    textContainer: {
        marginHorizontal: vars.PAD_HALF,
        marginTop: vars.PAD_HALF
    },
    icon: {
        width: 80,
        height: 80
    },
    actionsContainer: {
        flexDirection: 'row',
        marginTop: vars.PAD_NORMAL
    }
});