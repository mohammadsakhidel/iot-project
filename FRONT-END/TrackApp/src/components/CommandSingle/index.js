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

export default class CommandSindle extends Component {

    static contextType = AppContext;

    constructor(props) {
        super(props);

        // State:
        this.state = {
            isLoading: false,
            error: null,
            done: false,
            payload: ''
        };

        // Bindings:
        this.onSendCommandPress = this.onSendCommandPress.bind(this);

    }

    onSendCommandPress() {

        const { tracker, command } = this.props;

        this.setState({ error: null, done: false, isLoading: true }, async () => {
            try {

                Keyboard.dismiss();

                // Validate Input:
                const { validator, validationError } = this.props.command;
                if (validator) {
                    const valResult = validator(this.state.payload);
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
                const result = await CommandService.execute(dto, this.context.user.token);

                // Show Result:
                if (result.done) {
                    this.setState({ isLoading: false, done: true });
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
                    this.setState({ isLoading: false, done: false, error: errorMessage });
                }

            } catch (e) {
                this.setState({
                    isLoading: false,
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
                        isLoading={this.state.isLoading}
                        disabled={this.state.isLoading}
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
        color: vars.COLOR_GRAY_L1,
        textAlign: 'justify'
    },
    inputContainer: {
        marginTop: vars.PAD_DOUBLE
    }
});