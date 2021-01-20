import React, { Component } from 'react';
import { StyleSheet, View, ScrollView, Keyboard, Alert } from 'react-native';
import Text from '../Text';
import * as vars from '../../styles/vars';
import PrimaryButton from '../PrimaryButton';
import { Strings } from '../../i18n/strings';
import { getErrorMessage } from '../FlashMessageWrapper';
import FormError from '../FormError';
import FormSuccess from '../FormSuccess';
import CommandService from '../../api/services/command-service';
import AppContext from '../../helpers/app-context';
import * as ErrorCodes from '../../constants/error-codes';

export default class CommandGet extends Component {

    static contextType = AppContext;

    constructor(props) {
        super(props);

        // State:
        this.state = {
            isLoading: false,
            error: null,
            data: null
        };

        // Bindings:
        this.onSendCommandPress = this.onSendCommandPress.bind(this);

    }

    onSendCommandPress() {
        const { tracker, command } = this.props;

        this.setState({ error: null, data: null, isLoading: true }, async () => {
            try {
                // Call API:
                const dto = {
                    trackerId: tracker.id,
                    commandType: command.name,
                    payload: ""
                };
                const result = await CommandService.execute(dto, this.context.user.token);

                // Show Result:
                if (result.done) {
                    this.setState({ isLoading: false, data: result.data });
                } else {
                    let errorMessage = "";
                    switch (result.data) {
                        case ErrorCodes.TRACKER_OFFLINE:
                            errorMessage = Strings.ErrorCodeTrackerOffline
                            break;
                        case ErrorCodes.TRACKER_NO_REPLY:
                            errorMessage = Strings.ErrorCodeTrackerNoReply
                            break;
                        default:
                            errorMessage = result.data;
                            break;
                    }
                    this.setState({ isLoading: false, error: errorMessage });
                }

            } catch (e) {
                this.setState({
                    isLoading: false,
                    data: null,
                    error: getErrorMessage(e)
                });
            }
        });
    }

    render() {

        const { desc } = this.props;

        return (
            <View style={styles.container}>

                <ScrollView style={styles.scrollView}>

                    <Text style={styles.desc}>{desc}</Text>

                    <View style={styles.messageContainer}>
                        {this.state.error && (
                            <FormError error={this.state.error} />
                        )}

                        {this.state.data && (
                            <Text style={styles.data}>
                                {this.state.data}
                            </Text>
                        )}
                    </View>

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
        borderWidth: 1,
        borderStyle: 'dotted',
        borderColor: vars.COLOR_PRIMARY,
        backgroundColor: vars.COLOR_PRIMARY_L3,
        borderRadius: 10,

        padding: vars.PAD_NORMAL,
        color: vars.COLOR_PRIMARY_D1,
        textAlign: 'justify'
    },
    messageContainer: {
        marginTop: vars.PAD_DOUBLE
    },
    data: {
        borderWidth: 1,
        borderStyle: 'dotted',
        borderColor: vars.COLOR_GRAY_L2,
        backgroundColor: vars.COLOR_GRAY_LIGHTEST,

        padding: vars.PAD_NORMAL,
        color: vars.COLOR_GRAY
    }
});