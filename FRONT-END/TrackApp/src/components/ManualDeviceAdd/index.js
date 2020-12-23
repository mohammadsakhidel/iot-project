import React, { useState } from 'react';
import { StyleSheet } from 'react-native';
import { View } from 'react-native';
import { Input } from 'react-native-elements';
import Text from '../Text';
import PrimaryButton from '../PrimaryButton';
import LinkButton from '../LinkButton';
import { Strings } from '../../i18n/strings';
import * as vars from '../../styles/vars';
import * as GlobalStyles from '../../styles/global-styles';
import { Dimensions } from 'react-native';

export default function ManualDeviceAdd(props) {

    const [state, setState] = useState({
        serialNumber: ''
    });

    const {
        onAddPress,
        onCancelPress,
        onScanPress
    } = props;

    return (
        <View style={styles.container}>

            <Input
                label={Strings.SerialNumber}
                onChangeText={text => setState({ serialNumber: text })}
                defaultValue={state.serialNumber} />

            <View style={styles.buttons}>

                <PrimaryButton
                    title={Strings.Add}
                    icon="check"
                    style={styles.button}
                    onPress={() => onAddPress(state.serialNumber)}
                />

                <PrimaryButton
                    title={Strings.Cancel}
                    icon="times"
                    style={{ ...GlobalStyles.secondaryButton, ...styles.button }}
                    onPress={onCancelPress}
                />

            </View>
            <LinkButton
                icon="qrcode"
                title={Strings.ScanBarCode}
                onPress={onScanPress}
            />
        </View>
    );
}

const dim = Dimensions.get('window').width;

const styles = StyleSheet.create({
    container: {
        flex: 1,
        justifyContent: 'center',
        padding: vars.PAD_DOUBLE
    },
    buttons: {
        flexDirection: 'row',
        justifyContent: 'space-around',
        paddingVertical: vars.PAD_NORMAL
    },
    button: {
        width: dim * 0.27
    }
});