import React, { Component } from "react";
import { Picker } from '@react-native-picker/picker';
import { View, ScrollView, Keyboard, StyleSheet, Text } from "react-native";
import { languages } from '../../constants/collections';
import * as vars from '../../styles/vars';
import { Strings } from '../../i18n/strings';
import * as globalStyles from '../../styles/global-styles';
import moment from 'moment-timezone';
import _ from 'lodash';
import PrimaryButton from '../PrimaryButton';
import { getErrorMessage } from '../FlashMessageWrapper';
import FormError from '../FormError';
import FormSuccess from '../FormSuccess';
import * as Commands from '../../constants/command-names';
import AppContext from '../../helpers/app-context';
import CommandService from '../../api/services/command-service';

export default class CommandLZ extends Component {

    static contextType = AppContext;

    constructor(props) {
        super(props);

        // State:
        this.state = {
            isSending: false,
            isLoading: false,
            error: null,
            done: false,
            language: null,
            timezone: null
        };

        // Bindings:
        this.onSendCommandPress = this.onSendCommandPress.bind(this);

    }

    componentDidMount() {
        const { tracker } = this.props;

        this.setState({ isLoading: true }, async () => {
            try {

                // Call API:
                const result = await CommandService.getConfigs(tracker.id, this.context.user.token);
                const configs = result.data;
                const lz = (configs["lz"] ?? '').split(',');
                this.setState({ 
                    isLoading: false, 
                    language: lz[0],
                    timezone: lz[1]
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

                const areValid = this.state.language && this.state.timezone;
                if (!areValid) {
                    throw new Error(Strings.InvalidLanguageAndTZ);
                }

                // Call API:
                const langtz = `${this.state.language},${(this.state.timezone / 60).toFixed(2)}`;
                const dto = {
                    trackerId: tracker.id,
                    commandType: Commands.LANG_ZONE,
                    payload: langtz
                };
                const result = await CommandService.execute(dto, this.context.user.token, "lz");

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

        const allTimezones = moment.tz.names().map(n => ({
            name: `(${moment.tz(n).format('Z')}) ${n}`,
            offset: moment.tz(n).utcOffset()
        }));
        const timezones = _.uniqBy(allTimezones, (tz) => tz.offset)
            .sort((a, b) => a.offset > b.offset);

        return (
            <View style={styles.container}>

                <ScrollView style={styles.scrollView}>
                    <View style={styles.pickerContainer}>
                        <Text style={globalStyles.formLabel}>{Strings.Language}</Text>
                        <Picker mode="dropdown"
                            selectedValue={this.state.language}
                            onValueChange={(value) => this.setState({ language: value })}>
                            {!this.state.language && (
                                <Picker.Item label={Strings.SelectAnItem} value="" />
                            )}
                            {languages.map(l => (
                                <Picker.Item key={l.value} label={l.name} value={l.value}></Picker.Item>
                            ))}
                        </Picker>
                    </View>

                    <View style={styles.pickerContainer}>
                        <Text style={globalStyles.formLabel}>{Strings.Timezone}</Text>
                        <Picker mode="dialog"
                            selectedValue={this.state.timezone}
                            onValueChange={(value) => this.setState({ timezone: value })}>
                            {!this.state.timezone && (
                                <Picker.Item label={Strings.SelectAnItem} value="" />
                            )}
                            {timezones.map(tz => (
                                <Picker.Item key={tz.name} label={tz.name} value={tz.offset} />
                            ))}
                        </Picker>

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
    pickerContainer: {
        paddingBottom: vars.PAD_DOUBLE
    },
    scrollView: {
        flex: 1,
        padding: vars.PAD_DOUBLE
    },
    buttonContainer: {
        backgroundColor: vars.COLOR_GRAY_L3,
        padding: vars.PAD_DOUBLE
    }
});