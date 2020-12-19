import React from 'react';
import { View, StyleSheet, ActivityIndicator } from 'react-native';
import Text from '../Text';
import * as vars from '../../styles/vars';
import { Image } from 'react-native-elements';
import TrackerService from '../../api/services/tracker-service';
import { Strings } from '../../i18n/strings';
import LinkButton from '../LinkButton';

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
                    <LinkButton
                        icon="cog"
                        iconStyle={styles.actionIcon}
                        title={Strings.Configure}
                        titleStyle={styles.action}
                    />

                    <LinkButton
                        icon="trash"
                        iconStyle={{ ...styles.actionIcon, ...styles.removeActionIcon }}
                        title={Strings.Remove}
                        titleStyle={[styles.action, styles.removeAction]}
                    />
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
        width: 70,
        height: 70
    },
    actionsContainer: {
        flexDirection: 'row',
        marginTop: vars.PAD_HALF
    },
    action: {
        fontSize: vars.FS_BIT_SMALLER
    },
    removeAction: {
        color: vars.COLOR_ERROR
    },
    actionIcon: {
        fontSize: vars.ICO_SMALL
    },
    removeActionIcon: {
        color: vars.COLOR_ERROR
    }
});