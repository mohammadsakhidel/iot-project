import React, { Component } from 'react';
import { StyleSheet, View, Image, TouchableOpacity, Dimensions } from 'react-native';
import { BarCodeScanner as ExpoBarCodeScanner } from 'expo-barcode-scanner';
import Text from '../Text';
import { Strings } from '../../i18n/strings';
import * as vars from '../../styles/vars';

export default class BarCodeScanner extends Component {

    constructor(props) {
        super(props);

        // Bindings:
        this.handleBarCodeScanned = this.handleBarCodeScanned.bind(this);
    }

    handleBarCodeScanned(event) {
        const {
            onBarCodeScanned
        } = this.props;

        if (onBarCodeScanned)
            onBarCodeScanned(event);
    };

    static async requestPermissionsAsync() {
        return await ExpoBarCodeScanner.requestPermissionsAsync();
    }

    render() {

        const {
            onCancelPress,
            onManualPress
        } = this.props;

        return (
            <ExpoBarCodeScanner
                style={[StyleSheet.absoluteFill, styles.scanner]}
                onBarCodeScanned={this.handleBarCodeScanned}>

                <Text style={styles.scannerTitle}>
                    {Strings.ScanBarCode}
                </Text>
                <Image
                    source={require('../../styles/images/qr.png')}
                    style={styles.barCodePlaceholder}
                />
                <View style={styles.actions}>
                    <TouchableOpacity onPress={onManualPress}>
                        <Text style={styles.action}>
                            {Strings.InputManually}
                        </Text>
                    </TouchableOpacity>
                    <TouchableOpacity onPress={onCancelPress}>
                        <Text style={styles.action}>
                            {Strings.Cancel}
                        </Text>
                    </TouchableOpacity>
                </View>

            </ExpoBarCodeScanner>
        );
    }
}

const windowWidth = Dimensions.get('window').width;

const styles = StyleSheet.create({
    scanner: {
        flex: 1,
        width: Dimensions.get('screen').width,
        height: Dimensions.get('screen').height,
        justifyContent: 'center',
        alignItems: 'center'
    },
    scannerTitle: {
        color: vars.COLOR_GRAY_LIGHTEST,
        fontSize: vars.FS_XLARGE,
        textAlign: 'center'
    },
    actions: {
        alignSelf: 'stretch',
        marginHorizontal: vars.PAD_DOUBLE
    },
    action: {
        backgroundColor: vars.COLOR_GRAY_LIGHTEST,
        color: vars.COLOR_GRAY,
        borderRadius: 10,
        fontSize: vars.FS_BIT_LARGER,
        padding: vars.PAD_HALF,
        marginBottom: vars.PAD_NORMAL,
        textAlign: 'center'
    },
    barCodePlaceholder: {
        width: windowWidth * 0.65,
        height: windowWidth * 0.65,
        marginVertical: vars.PAD_DOUBLE
    }
});