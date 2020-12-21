import React, { Component } from 'react';
import { View, Button } from 'react-native';
import Text from '../Text';
import BarCodeScanner from '../BarCodeScanner';
import { StyleSheet, Dimensions, TouchableOpacity } from 'react-native';
import * as vars from '../../styles/vars';
import ScreenMessage from '../ScreenMessage';
import { Strings } from '../../i18n/strings';
import * as GlobalStyles from '../../styles/global-styles';
import { Image } from 'react-native';

export default class TrackerAddScreen extends Component {
    constructor(props) {
        super(props);

        this.state = {
            hasPermission: null,
            scanned: false,
            barCodeType: '',
            barCodeValue: ''
        };

        // Bindings: 
        this.onBarCodeScanned = this.onBarCodeScanned.bind(this);

    }

    async componentDidMount() {
        const { status } = await BarCodeScanner.requestPermissionsAsync();
        this.setState({
            hasPermission: status == 'granted'
        });
    }

    render() {
        return (
            this.state.hasPermission
                ? (
                    <View style={styles.container}>

                        <BarCodeScanner 
                            onBarCodeScanned={this.onBarCodeScanned}
                            onManualPress={this.onManualPress}
                            onCancelPress={this.onCancelPress}
                        />

                        <View style={styles.panel}>
                            {this.state.scanned &&
                                <Button
                                    title="Tap to scan again..."
                                    onPress={() => this.setState({ scanned: false })}
                                />
                            }
                        </View>
                    </View>
                )
                : (
                    <View style={{ flex: 1, justifyContent: 'center' }}>
                        {this.state.hasPermission === false
                            ? (
                                <ScreenMessage icon="exclamation">
                                    {Strings.PermissionNoAccessCamera}
                                </ScreenMessage>
                            )
                            : (
                                <ScreenMessage icon="exclamation">
                                    {Strings.PermissionRequestingCamera}
                                </ScreenMessage>
                            )}
                    </View>
                )
        );
    }

    onCancelPress() {
        console.log("Cancelled");
    }

    onManualPress() {
        console.log("Manual Pressed...");
    }

    onBarCodeScanned({ type, data }) {
        console.log(`Type: ${type}, Value: ${data}`);
        //this.setState({ scanned: true, barCodeType: type, barCodeValue: data });
    }
}

const styles = StyleSheet.create({
    container: {
        flex: 1,
        padding: 0
    },
    panel: {
        position: 'absolute',
        bottom: 0,
        left: 0,
        right: 0
    }
});