import React, { useContext, useRef } from 'react';
import Text from '../Text';
import QRCode from '../QRCode';
import { View, StyleSheet } from 'react-native';
import * as vars from '../../styles/vars';
import TrackerService from '../../api/services/tracker-service';
import { Avatar } from 'react-native-elements';
import AppContext from '../../helpers/app-context';
import { Strings } from '../../i18n/strings';
import { format } from '../../utils/text-util';

export default function QRCodeScreen(props) {

    const {
        route
    } = props;
    const tracker = route.params;

    return (
        <View style={styles.container}>
            <View style={styles.panel}>

                <View style={styles.avatarContainer}>
                    <Avatar
                        rounded
                        size="large"
                        source={{ uri: TrackerService.getIconUrl(tracker) }}
                        containerStyle={styles.avatar}
                    />
                </View>

                <Text style={styles.title}>
                    {tracker.displayName}
                </Text>

                <View style={styles.qrContainer}>
                    <QRCode
                        style={styles.qr}
                        value={tracker.serialNumber}
                        size={150}
                    />
                </View>

            </View>

            <Text style={styles.desc}>
                {format(Strings.TrackerQRCodePageDesc, tracker.displayName)}
            </Text>

        </View>
    );
}

const styles = StyleSheet.create({
    container: {
        flex: 1,
        justifyContent: 'center'
    },
    panel: {
        backgroundColor: vars.COLOR_GRAY_LIGHTEST,
        borderRadius: 10,
        marginHorizontal: vars.PAD_TRIPPLE,
        alignItems: 'center',

    },
    avatarContainer: {
        marginTop: -40
    },
    qrContainer: {
        marginBottom: vars.PAD_TRIPPLE,
        marginTop: vars.PAD_NORMAL
    },
    avatar: {
        backgroundColor: vars.COLOR_GRAY_LIGHTEST,
        padding: 3
    },
    title: {
        marginTop: vars.PAD_NORMAL,
        fontWeight: 'bold',
        fontSize: vars.FS_BIT_LARGER
    },
    desc: {
        color: vars.COLOR_GRAY_L1,
        textAlign: 'center',
        marginHorizontal: vars.PAD_DOUBLE,
        marginTop: vars.PAD_NORMAL,
        fontSize: vars.FS_BIT_SMALLER
    }
});