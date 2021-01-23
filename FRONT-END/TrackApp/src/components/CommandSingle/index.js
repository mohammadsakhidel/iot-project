import React, { Component } from 'react';
import { StyleSheet, View, ScrollView, Keyboard } from 'react-native';
import Text from '../Text';
import { Input } from 'react-native-elements';
import * as vars from '../../styles/vars';
import PrimaryButton from '../PrimaryButton';
import { Strings } from '../../i18n/strings';
import { getErrorMessage } from '../FlashMessageWrapper';
import FormError from '../FormError';
import FormSuccess from '../FormSuccess';
import CommandService from '../../api/services/command-service';
import AppContext from '../../helpers/app-context';
import * as ErrorCodes from '../../constants/error-codes';
import * as Validators from '../../utils/command-validators';

export default class CommandSindle extends Component {

    static contextType = AppContext;

    constructor(props) {
        super(props);

        // State:
        this.state = {
            isSending: false,
            isLoading: false,
            error: null,
            done: false,
            payload: ''
        };

        // Bindings:
        this.onSendCommandPress = this.onSendCommandPress.bind(this);

    }

    componentDidMount() {

        const { tracker, command } = this.props;

        this.setState({ isLoading: true }, async () => {
            try {

                // Call API:
                const result = await CommandService.getConfigs(tracker.id, this.context.user.token);
                const configs = result.data;
                this.setState({ isLoading: false, payload: configs ? configs[command.configField] : '' });

            } catch (e) {
                this.setState({ isLoading: false });
                showError(e);
            }
        });
    }

    onSendCommandPress() {

        const { tracker, command } = this.props;

        this.setState({ error: null, done: false, isSending: true }, async () => {
            try {

                Keyboard.dismiss();

                // Validate Input:
                const { validator, validationError } = this.props.command;
                if (validator) {
                    const validatorFunc = Validators[validator];
                    const valResult = validatorFunc(this.state.payload);
                    if (!valResult) {
                        throw new Error(validationError);
                    }
                }

                // Call API:
                const dto = {
                    trackerId: tracker.id,
                    commandType: command.name,
                    payload: this.state.payload
                };
                const result = await CommandService.execute(dto, this.context.user.token, command.url);

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
        const { label, inputType } = this.props.command;
        const { desc } = this.props;

        return (
            <View style={styles.container}>

                <ScrollView style={styles.scrollView}>

                    <Text style={styles.desc}>{desc}</Text>

                    <View style={styles.inputContainer}>
                        {inputType === "string" && (
                            <Input
                                label={label}
                                onChangeText={(text) => this.setState({ payload: text })}
                                value={(this.state.isLoading ? Strings.Loading : this.state.payload)}
                                disabled={this.state.isLoading}
                            />
                        )}
                    </View>

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

            </View>
        );
    }



}

const styles = StyleSheet.create({
    container: {
        flex: 1
    },
    scrollView: {
        flex: 1,
        padding: vars.PAD_DOUBLE
    },
    buttonContainer: {
        backgroundColor: vars.COLOR_GRAY_L3,
        padding: vars.PAD_DOUBLE
    },
    desc: {
        borderWidth: 1,
        borderStyle: 'dotted',
        borderColor: vars.COLOR_PRIMARY,
        backgroundColor: vars.COLOR_PRIMARY_L3,
        borderRadius: 10,

        padding: vars.PAD_NORMAL,
        color: vars.COLOR_PRIMARY_D1,
        textAlign: 'justify'
    },
    inputContainer: {
        marginTop: vars.PAD_DOUBLE
    }
});