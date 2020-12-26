import React, { Component } from 'react';
import { View } from 'react-native';
import Text from '../Text';
import BarCodeScanner from '../BarCodeScanner';
import { StyleSheet } from 'react-native';
import * as vars from '../../styles/vars';
import ScreenMessage from '../ScreenMessage';
import { Strings } from '../../i18n/strings';
import * as GlobalStyles from '../../styles/global-styles';
import { NavigationContext } from '@react-navigation/native';
import * as RouteNames from '../../constants/route-names';
import Loading from '../Loading';
import LinkButton from '../LinkButton';
import { getErrorMessage, showError } from '../FlashMessageWrapper';
import TrackerService from '../../api/services/tracker-service';
import { connect } from 'react-redux';
import * as Actions from '../../redux/actions';
import ManualDeviceAdd from '../ManualDeviceAdd';

class TrackerAddScreen extends Component {

    constructor(props) {
        super(props);

        this.state = {
            hasPermission: null,
            scanned: false,
            isLoading: false,
            error: '',
            method: 'barcode'
        };

        // Bindings: 
        this.onBarCodeScanned = this.onBarCodeScanned.bind(this);
        this.onRescanPress = this.onRescanPress.bind(this);
        this.onManualPress = this.onManualPress.bind(this);
        this.onScanPress = this.onScanPress.bind(this);
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
                    <NavigationContext.Consumer>
                        {navigation => (
                            <View style={styles.container}>

                                {this.state.method == 'manual'
                                    ? (
                                        <ManualDeviceAdd
                                            onAddPress={(data) => {
                                                this.onBarCodeScanned({ data, navigation });
                                            }}
                                            onCancelPress={() => this.onCancelPress(navigation)}
                                            onScanPress={this.onScanPress}
                                        />
                                    )
                                    : (
                                        <BarCodeScanner
                                            onBarCodeScanned={(event) => {
                                                event.navigation = navigation;
                                                this.onBarCodeScanned(event);
                                            }}
                                            onManualPress={this.onManualPress}
                                            onCancelPress={() => this.onCancelPress(navigation)}
                                        />
                                    )
                                }

                                {this.state.scanned ? (
                                    <View style={styles.panel}>
                                        {this.state.isLoading && <Loading size="small" />}
                                        {this.state.error ? (
                                            <View>
                                                <Text style={{ ...GlobalStyles.error, textAlign: 'center' }}>{this.state.error}</Text>
                                                <LinkButton
                                                    title={(this.state.method === 'barcode' ? Strings.Rescan : Strings.Retry)}
                                                    icon="refresh"
                                                    onPress={this.onRescanPress}
                                                />
                                            </View>
                                        ) : null}
                                    </View>
                                ) : null}
                            </View>
                        )}
                    </NavigationContext.Consumer>
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

    onManualPress() {
        this.setState({ method: 'manual' });
    }

    onScanPress() {
        this.setState({ method: 'barcode' });
    }

    onBarCodeScanned({ data, navigation }) {
        if (!this.state.scanned) {
            this.setState({ scanned: true, isLoading: true }, async () => {
                try {

                    const result = await TrackerService.add(data);
                    if (!result.done)
                        throw new Error(result.data);

                    const {
                        trackers,
                        setTrackers
                    } = this.props;

                    trackers.push(result.data);
                    setTrackers(trackers);
                    navigation.navigate(RouteNames.HOME_LOGIN_SWITCH);

                } catch (e) {
                    this.setState({ isLoading: false, error: getErrorMessage(e) });
                }
            });
        }
    }

    onCancelPress(navigation) {
        try {
            navigation.navigate(RouteNames.HOME_LOGIN_SWITCH);
        } catch (e) {
            showError(e);
        }
    }

    onRescanPress() {
        try {
            this.setState({
                scanned: false,
                isLoading: false,
                error: ''
            });
        } catch (e) {
            showError(e);
        }
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
        right: 0,
        padding: vars.PAD_NORMAL,
        alignItems: 'center',
        backgroundColor: vars.COLOR_GRAY_LIGHTEST
    },
    loading: {

    }
});

const mapStateToProps = (state) => {
    return {
        trackers: state.trackers
    };
};

const mapDispatchToProps = (dispatch) => {
    return {
        setTrackers: (trackers) => {
            dispatch(Actions.setTrackers(trackers));
        }
    };
};

export default connect(mapStateToProps, mapDispatchToProps)(TrackerAddScreen);