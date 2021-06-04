import React, { Component } from "react";
import { View, ScrollView, StyleSheet, Keyboard } from 'react-native';
import Text from "../Text";
import * as vars from '../../styles/vars';
import { Input } from 'react-native-elements';
import { Strings } from '../../i18n/strings';
import PrimaryButton from '../PrimaryButton';
import * as globalStyles from '../../styles/global-styles';
import { getErrorMessage } from '../FlashMessageWrapper';
import * as Validators from '../../utils/command-validators';
import FormError from '../FormError';
import FormSuccess from '../FormSuccess';
import * as Commands from '../../constants/command-names';
import AppContext from '../../helpers/app-context';
import CommandService from '../../api/services/command-service';

export default class CommandSOS extends Component {

    static contextType = AppContext;

    constructor(props) {
        super(props);

        // Bindings:
        this.onSendCommandPress = this.onSendCommandPress.bind(this);

        // State:
        this.state = {
            isSending: false,
            isLoading: false,
            error: null,
            done: false,
            sos1: '',
            sos2: '',
            sos3: ''
        };
    }

    componentDidMount() {

        const { tracker } = this.props;

        this.setState({ isLoading: true }, async () => {
            try {

                // Call API:
                const result = await CommandService.getConfigs(tracker.id, this.context.user.token);
                const configs = result.data;
                const sosNumbers = (configs["sos"] ?? '').split(',');
                this.setState({ 
                    isLoading: false, 
                    sos1: sosNumbers[0] ?? '',
                    sos2: sosNumbers[1] ?? '',
                    sos3: sosNumbers[2] ?? ''
                });

            } catch (e) {
                this.setState({ isLoading: false });
                showError(e);
            }
        });

    }

    onSendCommandPress() {
        const { tracker } = this.props;
        this.setState({ error: null, done: false, isSending: true }, async () => {
            try {

                Keyboard.dismiss();

                const validatorFunc = Validators.phoneNumberValidator;
                const areValid = ((!this.state.sos1 || validatorFunc(this.state.sos1)) &&
                    (!this.state.sos2 || validatorFunc(this.state.sos2)) &&
                    (!this.state.sos3 || validatorFunc(this.state.sos3)));
                if (!areValid) {
                    throw new Error(Strings.InvalidPhoneNumberError);
                }

                // Call API:
                const sosNumbersString = `${this.state.sos1},${this.state.sos2},${this.state.sos3}`;
                const dto = {
                    trackerId: tracker.id,
                    commandType: Commands.SOS_NUMBERS,
                    payload: sosNumbersString
                };
                const result = await CommandService.execute(dto, this.context.user.token, "sos");

                // Show Result:
                if (result.done) {
                    this.setState({ isSending: false, done: true });
                } else {
                    let errorMessage = "";
                    switch (result.data) {
                        case ErrorCodes.TRACKER_OFFLINE:
                            errorMessage = Strings.ErrorCodeTrackerOffline
                            break;
                        default:
                            errorMessage = result.data;
                            break;
                    }
                    this.setState({ isSending: false, done: false, error: errorMessage });
                }

            } catch (e) {
                this.setState({
                    isSending: false,
                    done: false,
                    error: getErrorMessage(e)
                });
            }
        });
    }

    render() {
        return (
            <View style={styles.container}>
                <ScrollView style={styles.scrollViewer}>

                    <Text style={{ ...globalStyles.commandDesc, ...styles.desc }}>{Strings.SOSDesc}</Text>

                    <Input
                        label={Strings.SOS1}
                        placeholder={Strings.PhoneNumber}
                        leftIcon={{ name: 'phone', color: vars.COLOR_PRIMARY, size: vars.ICO_BIT_SMALLER }}
                        leftIconContainerStyle={styles.leftIconContainer}
                        value={(this.state.isLoading ? Strings.Loading : this.state.sos1)}
                        disabled={this.state.isLoading}
                        onChangeText={(text) => this.setState({ sos1: text })}
                    />

                    <Input
                        labelStyle={styles.inputLabel}
                        label={Strings.SOS2}
                        placeholder={Strings.PhoneNumber}
                        leftIcon={{ name: 'phone', color: vars.COLOR_PRIMARY, size: vars.ICO_BIT_SMALLER }}
                        leftIconContainerStyle={styles.leftIconContainer}
                        value={(this.state.isLoading ? Strings.Loading : this.state.sos2)}
                        disabled={this.state.isLoading}
                        onChangeText={(text) => this.setState({ sos2: text })}
                    />

                    <Input
                        labelStyle={styles.inputLabel}
                        label={Strings.SOS3}
                        placeholder={Strings.PhoneNumber}
                        leftIcon={{ name: 'phone', color: vars.COLOR_PRIMARY, size: vars.ICO_BIT_SMALLER }}
                        leftIconContainerStyle={styles.leftIconContainer}
                        value={(this.state.isLoading ? Strings.Loading : this.state.sos3)}
                        disabled={this.state.isLoading}
                        onChangeText={(text) => this.setState({ sos3: text })}
                    />

                    {this.state.error && (
                        <FormError error={this.state.error} />
                    )}

                    {this.state.done && (
                        <FormSuccess message={Strings.CommandSentMessage} />
                    )}

                </ScrollView>

                <View style={styles.buttonContainer}>
                    <PrimaryButton
                        icon="check"
                        title={Strings.SendCommand}
                        onPress={this.onSendCommandPress}
                        isLoading={this.state.isSending}
                        disabled={this.state.isSending}
                    />
                </View>

            </View >
        );
    }
}

const styles = StyleSheet.create({
    container: {
        flex: 1
    },
    scrollViewer: {
        flex: 1,
        padding: vars.PAD_DOUBLE
    },
    leftIconContainer: {
        margin: 5
    },
    inputLabel: {
        marginTop: vars.PAD_NORMAL
    },
    buttonContainer: {
        backgroundColor: vars.COLOR_GRAY_L3,
        padding: vars.PAD_DOUBLE
    },
    desc: {
        marginBottom: vars.PAD_DOUBLE
    }
});