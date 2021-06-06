import React, { Component } from 'react';
import { View, StyleSheet, Text, ScrollView, Keyboard, Alert } from 'react-native';
import { Input } from 'react-native-elements';
import * as globalStyles from '../../styles/global-styles';
import { Strings } from '../../i18n/strings';
import FormError from '../FormError';
import FormSuccess from '../FormSuccess';
import PrimaryButton from '../PrimaryButton';
import * as Validators from '../../utils/command-validators';
import CommandService from '../../api/services/command-service';
import * as vars from '../../styles/vars';
import { getErrorMessage, showError } from '../FlashMessageWrapper';
import * as Commands from '../../constants/command-names';
import AppContext from '../../helpers/app-context';

export default class CommandSetServer extends Component {

    static contextType = AppContext;

    constructor(props) {
        super(props);

        // Bindings:
        this.onSendCommandPress = this.onSendCommandPress.bind(this);
        this.sendCommand = this.sendCommand.bind(this);

        // State:
        this.state = {
            isSending: false,
            isLoading: false,
            error: null,
            done: false,
            server: '',
            port: ''
        };

    }

    onSendCommandPress() {
        try {

            Alert.alert(
                Strings.AreYouSure,
                Strings.AreYouSureChangeServer,
                [
                    {
                        text: Strings.Yes,
                        onPress: () => this.sendCommand()
                    },
                    {
                        text: Strings.Cancel,
                        onPress: () => { }
                    }
                ]
            );

        } catch (e) {
            showError(e);
        }
    }

    sendCommand() {
        const { tracker } = this.props;
        this.setState({ error: null, done: false, isSending: true }, async () => {
            try {

                Keyboard.dismiss();

                const isServerValid = this.state.server && Validators.serverAddressValidator(this.state.server);
                if (!isServerValid)
                    throw new Error(Strings.InvalidServerName);

                const isPortValid = this.state.port && Validators.numberValidator(this.state.port);
                if (!isPortValid)
                    throw new Error(Strings.InvalidPortNumber);


                // Call API:
                const serverPortString = `${this.state.server},${this.state.port}`;
                const dto = {
                    trackerId: tracker.id,
                    commandType: Commands.IP_PORT,
                    payload: serverPortString
                };
                const result = await CommandService.execute(dto, this.context.user.token, "server");

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

    componentDidMount() {
        const { tracker } = this.props;

        this.setState({ isLoading: true }, async () => {
            try {

                // Call API:
                const result = await CommandService.getConfigs(tracker.id, this.context.user.token);
                const configs = result.data;
                const address = (configs["server"] ?? '').split(',');
                this.setState({
                    isLoading: false,
                    server: address[0] ?? '',
                    port: address[1] ?? ''
                });

            } catch (e) {
                this.setState({ isLoading: false });
                showError(e);
            }
        });
    }

    render() {
        return (
            <View style={styles.container}>
                <ScrollView style={styles.scrollViewer}>

                    <Text style={{ ...globalStyles.commandDesc, ...styles.desc }}>{Strings.SetServerDesc}</Text>

                    <Input
                        label={Strings.IPOrDomain}
                        placeholder={Strings.Address}
                        value={(this.state.isLoading ? Strings.Loading : this.state.server)}
                        disabled={this.state.isLoading}
                        onChangeText={(text) => this.setState({ server: text })}
                    />

                    <Input
                        labelStyle={styles.inputLabel}
                        label={Strings.Port}
                        placeholder={Strings.Number}
                        value={(this.state.isLoading ? Strings.Loading : this.state.port)}
                        disabled={this.state.isLoading}
                        onChangeText={(text) => this.setState({ port: text })}
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