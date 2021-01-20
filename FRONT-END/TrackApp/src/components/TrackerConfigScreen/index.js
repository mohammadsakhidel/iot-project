import React, { Component } from 'react';
import { View, StyleSheet, ScrollView, Alert } from 'react-native';
import Text from '../Text';
import { Avatar, ListItem } from 'react-native-elements';
import Icon from '../Icon';
import TrackerService from '../../api/services/tracker-service';
import * as vars from '../../styles/vars';
import { Strings } from '../../i18n/strings';
import SettingsItem from '../SettingsItem';
import SettingsSection from '../SettingsSection';
import AppContext from '../../helpers/app-context';
import { NavigationContext } from '@react-navigation/native';
import * as RouteNames from '../../constants/route-names';
import { connect } from 'react-redux';
import * as Actions from '../../redux/actions';
import TrackerStatus from '../TrackerStatus';
import * as Commands from '../../constants/command-names';

class TrackerConfigScreen extends Component {

    static contextType = AppContext;

    constructor(props) {
        super(props);

        // Bindings:
        this.onRemovePress = this.onRemovePress.bind(this);
        this.removeTrackerFunc = this.removeTrackerFunc.bind(this);
    }

    render() {
        const {
            route,
            connections
        } = this.props;
        const tracker = route.params;

        return (
            <NavigationContext.Consumer>
                {navigation => (
                    <View style={styles.container}>

                        <ListItem bottomDivider onPress={() => { }}>
                            <Avatar
                                rounded
                                size="large"
                                source={{ uri: TrackerService.getIconUrl(tracker) }}
                            />
                            <ListItem.Content>
                                <ListItem.Title>
                                    {tracker.displayName}
                                </ListItem.Title>
                                <TrackerStatus
                                    status={(connections[tracker.id]?.status ?? tracker.status)}
                                    lastConnection={(connections[tracker.id]?.lastConnection ?? tracker.lastConnection)}
                                />
                            </ListItem.Content>
                            <ListItem.Chevron
                                name="edit"
                                type="font-awesome"
                                size={vars.ICO_NORMAL}
                                color={vars.COLOR_SECONDARY_L3}
                            />
                        </ListItem>

                        <ScrollView style={styles.settingsContainer}>

                            <SettingsSection title={Strings.PrivacySettings}>
                                <SettingsItem icon="qrcode" onPress={() => {
                                    navigation.navigate(RouteNames.QR_CODE_SCREEN, tracker);
                                }}>
                                    {Strings.QRCode}
                                </SettingsItem>
                                <SettingsItem icon="users" onPress={() => {
                                    navigation.navigate(RouteNames.PERMITTED_USERS_SCREEN, {
                                        tracker: tracker
                                    });
                                }}>
                                    {Strings.AllowedUsers}
                                </SettingsItem>
                                <SettingsItem icon="key" onPress={() => {
                                    navigation.navigate(RouteNames.COMMAND_SCREEN, {
                                        tracker: tracker,
                                        pageTitle: Strings.DevicePassword,
                                        desc: Strings.DevicePasswordDesc,
                                        command: {
                                            name: Commands.PASSWORD,
                                            type: "single",
                                            label: `${Strings.DevicePassword}:`,
                                            inputType: "string",
                                            validator: "devicePasswordValidator",
                                            validationError: Strings.DevicePasswordValidationError
                                        }
                                    });
                                }}>
                                    {Strings.DevicePassword}
                                </SettingsItem>
                            </SettingsSection>

                            <SettingsSection title={Strings.DeviceSettings}>
                                <SettingsItem icon="mobile" onPress={() => {
                                    navigation.navigate(RouteNames.COMMAND_SCREEN, {
                                        tracker: tracker,
                                        pageTitle: Strings.CenterNumber,
                                        desc: Strings.CenterNumberDesc,
                                        command: {
                                            name: Commands.CENTER_NUMBER,
                                            type: "single",
                                            label: `${Strings.CenterNumber}:`,
                                            inputType: "string",
                                            validator: "phoneNumberValidator",
                                            validationError: Strings.CenterNumberValidationError
                                        }
                                    });
                                }}>
                                    {Strings.CenterNumber}
                                </SettingsItem>
                                <SettingsItem icon="address-book" onPress={() => { }}>
                                    {Strings.Contacts}
                                </SettingsItem>
                                <SettingsItem icon="exclamation-circle" onPress={() => { }}>
                                    {Strings.SOSNumbers}
                                </SettingsItem>
                                <SettingsItem icon="bell" onPress={() => { }}>
                                    {Strings.AlarmSettings}
                                </SettingsItem>
                                <SettingsItem icon="bell-slash" onPress={() => { }}>
                                    {Strings.NoDisturbanceTime}
                                </SettingsItem>
                                <SettingsItem icon="globe" onPress={() => { }}>
                                    {Strings.ServerAndPortNumber}
                                </SettingsItem>
                                <SettingsItem icon="history" onPress={() => {
                                    navigation.navigate(RouteNames.COMMAND_SCREEN, {
                                        tracker: tracker,
                                        pageTitle: Strings.UploadInterval,
                                        desc: Strings.UploadIntervalDesc,
                                        command: {
                                            name: Commands.UPLOAD_INTERVAL,
                                            type: "single",
                                            label: `${Strings.UploadIntervalLabel}:`,
                                            inputType: "string",
                                            validator: "numberValidator",
                                            validationError: Strings.UploadIntervalValidationError
                                        }
                                    });
                                }}>
                                    {Strings.UploadInterval}
                                </SettingsItem>
                                <SettingsItem icon="language" onPress={() => { }}>
                                    {Strings.LanguageAndTimezone}
                                </SettingsItem>
                            </SettingsSection>

                            <SettingsSection title={Strings.Commands}>
                                <SettingsItem icon="info-circle" onPress={() => { }}>
                                    {Strings.CheckVersion}
                                </SettingsItem>
                                <SettingsItem icon="phone" onPress={() => {
                                    navigation.navigate(RouteNames.COMMAND_SCREEN, {
                                        tracker: tracker,
                                        pageTitle: Strings.MakeCall,
                                        desc: Strings.MakeCallDesc,
                                        command: {
                                            name: Commands.MAKE_CALL,
                                            type: "single",
                                            label: `${Strings.PhoneNumber}:`,
                                            inputType: "string",
                                            validator: "phoneNumberValidator",
                                            validationError: Strings.MakeCallValidationError
                                        }
                                    });
                                }}>
                                    {Strings.MakeCall}
                                </SettingsItem>
                                <SettingsItem icon="phone-square" onPress={() => {
                                    navigation.navigate(RouteNames.COMMAND_SCREEN, {
                                        tracker: tracker,
                                        pageTitle: Strings.FindDevice,
                                        desc: Strings.FindDeviceDesc,
                                        showAlert: false,
                                        command: {
                                            name: Commands.FIND,
                                            type: "zero"
                                        }
                                    });
                                }}>
                                    {Strings.FindDevice}
                                </SettingsItem>
                                <SettingsItem icon="bullhorn" onPress={() => {
                                    navigation.navigate(RouteNames.COMMAND_SCREEN, {
                                        tracker: tracker,
                                        pageTitle: Strings.Wakeup,
                                        desc: Strings.WakeupDesc,
                                        showAlert: false,
                                        command: {
                                            name: Commands.WAKEUP,
                                            type: "zero"
                                        }
                                    });
                                }}>
                                    {Strings.Wakeup}
                                </SettingsItem>
                                <SettingsItem icon="repeat" onPress={() => {
                                    navigation.navigate(RouteNames.COMMAND_SCREEN, {
                                        tracker: tracker,
                                        pageTitle: Strings.Restart,
                                        desc: Strings.RestartDesc,
                                        showAlert: true,
                                        alertTitle: Strings.AreYouSure,
                                        alertMessage: Strings.RestartAlert,
                                        command: {
                                            name: Commands.RESTART,
                                            type: "zero"
                                        }
                                    });
                                }}>
                                    {Strings.Restart}
                                </SettingsItem>
                                <SettingsItem icon="power-off" onPress={() => {
                                    navigation.navigate(RouteNames.COMMAND_SCREEN, {
                                        tracker: tracker,
                                        pageTitle: Strings.PowerOff,
                                        desc: Strings.PowerOffDesc,
                                        showAlert: true,
                                        alertTitle: Strings.AreYouSure,
                                        alertMessage: Strings.PowerOffAlert,
                                        command: {
                                            name: Commands.POWEROFF,
                                            type: "zero"
                                        }
                                    });
                                }}>
                                    {Strings.PowerOff}
                                </SettingsItem>
                                <SettingsItem icon="minus-circle" onPress={() => {
                                    navigation.navigate(RouteNames.COMMAND_SCREEN, {
                                        tracker: tracker,
                                        pageTitle: Strings.ResetFactory,
                                        desc: Strings.ResetFactoryDesc,
                                        showAlert: true,
                                        alertTitle: Strings.AreYouSure,
                                        alertMessage: Strings.ResetFactoryAlert,
                                        command: {
                                            name: Commands.RESET_FACTORY,
                                            type: "zero"
                                        }
                                    });
                                }}>
                                    {Strings.ResetFactory}
                                </SettingsItem>
                            </SettingsSection>

                            <SettingsSection title={Strings.Actions}>
                                <SettingsItem icon="trash" chevronShown={false}
                                    iconColor={vars.COLOR_ERROR}
                                    textColor={vars.COLOR_ERROR}
                                    onPress={() => this.onRemovePress(navigation)}>

                                    {Strings.Remove}

                                </SettingsItem>
                            </SettingsSection>

                            <View>
                                <Text style={styles.footer}>
                                    {Strings.Footer}
                                </Text>
                            </View>

                        </ScrollView>

                    </View>
                )}
            </NavigationContext.Consumer>
        );
    }

    onRemovePress(navigation) {

        const {
            route,
            removeTracker
        } = this.props;
        const item = route.params;

        Alert.alert(
            item.displayName,
            Strings.RemoveTrackerSureMessage,
            [
                {
                    text: Strings.Yes,
                    onPress: () => {
                        this.setState({ isLoading: true }, async () => {
                            try {
                                await this.removeTrackerFunc(item);
                                removeTracker(item.id);
                                navigation.navigate(RouteNames.HOME_LOGIN_SWITCH);
                            } catch (e) {
                                this.setState({ isLoading: false });
                                showError(e);
                            }
                        });
                    }
                },
                {
                    text: Strings.Cancel,
                    onPress: () => { }
                }
            ]
        );

    };

    async removeTrackerFunc(tracker) {
        await TrackerService.remove(tracker.id, this.context.user.token);
    }


}

const styles = StyleSheet.create({
    container: {
        flex: 1
    },
    settingsContainer: {
        flex: 1
    },
    footer: {
        color: vars.COLOR_GRAY_L2,
        padding: vars.PAD_DOUBLE,
        textAlign: 'center',
        fontSize: vars.FS_BIT_LARGER
    }
});

const mapStateToProps = (state) => {
    return {
        trackers: state.trackers,
        connections: state.connections
    };
};

const mapDispatchToProps = (dispatch) => {
    return {
        removeTracker: (trackerId) => {
            dispatch(Actions.removeTracker(trackerId));
        }
    };
};

export default connect(mapStateToProps, mapDispatchToProps)(TrackerConfigScreen);