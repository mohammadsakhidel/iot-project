import React from 'react';
import { View, StyleSheet } from "react-native";
import * as GlobalStyles from '../../styles/global-styles';
import Icon from '../Icon';
import Text from '../Text';
import { Strings } from '../../i18n/strings';
import * as vars from '../../styles/vars';
import { formatDateString } from '../../utils/text-util';

export default function TrackerStatus(props) {

    const {
        status,
        lastConnection
    } = props;

    switch (status) {
        case null:
        case undefined:
            return (
                <Text style={GlobalStyles.smallText}>
                    {Strings.StatusLoading}
                </Text>
            );
        case 'online':
            return (
                <View style={{ flexDirection: 'row', alignItems: 'center' }}>
                    <Icon name="dot-circle" style={styles.onlineIcon} />
                    <Text style={styles.online}>
                        {Strings.Online}
                    </Text>
                </View>
            );
        case 'offline':
            return (
                <View style={{ flexDirection: 'row', alignItems: 'center' }}>
                    <Icon name="dot-circle" style={styles.offlineIcon} />
                    <Text style={styles.offline}>
                        {
                            Strings.Offline +
                            (lastConnection
                                ? ' ' + Strings.Since + ' ' + formatDateString(lastConnection)
                                : ''
                            )
                        }
                    </Text>
                </View>
            );
        case 'error':
            return (
                <View style={{ flexDirection: 'row', alignItems: 'center' }}>
                    <Text style={styles.errorStatus}>
                        {Strings.ErrorCheckingStatus}
                    </Text>
                </View>
            );
        default:
            return null;
    }
}

const styles = StyleSheet.create({
    online: {
        ...GlobalStyles.smallText,
        ...GlobalStyles.success,
        marginHorizontal: vars.PAD_HALF,
        fontWeight: 'bold'
    },
    onlineIcon: {
        ...GlobalStyles.success,
        fontSize: vars.ICO_TINY
    },
    offline: {
        ...GlobalStyles.smallText,
        marginHorizontal: vars.PAD_HALF
    },
    offlineIcon: {
        color: vars.COLOR_GRAY_L1,
        fontSize: vars.ICO_TINY
    },
    errorStatus: {
        ...GlobalStyles.smallText,
        ...GlobalStyles.error,
        marginHorizontal: vars.PAD_HALF
    }
});