import React, { Component } from "react";
import { View, ScrollView, StyleSheet, Keyboard } from "react-native";
import * as vars from '../../styles/vars';
import ReminderItem from "../ReminderItem";
import { showError } from '../FlashMessageWrapper';
import PrimaryButton from '../PrimaryButton';
import { Strings } from '../../i18n/strings';
import BottomSheet from '../BottomSheet';
import ReminderEditor from '../ReminderEditor';
import { getErrorMessage } from '../FlashMessageWrapper';
import * as Commands from '../../constants/command-names';
import * as ErrorCodes from '../../constants/error-codes';
import CommandService from '../../api/services/command-service';
import FormError from '../FormError';
import FormSuccess from '../FormSuccess';
import appContext from "../../helpers/app-context";

export default class CommandAlarmClock extends Component {

    static contextType = appContext;

    constructor(props) {
        super(props);

        // Bindings: 
        this.onSelectedChange = this.onSelectedChange.bind(this);
        this.onSendCommandPress = this.onSendCommandPress.bind(this);
        this.onAlarmPress = this.onAlarmPress.bind(this);
        this.onAlarmEditorConfirmPress = this.onAlarmEditorConfirmPress.bind(this);

        // State:
        this.state = {
            isSending: false,
            isLoading: false,
            error: null,
            done: false,
            alarms: [{}, {}, {}],
            editingAlarmIndex: -1
        };

    }

    componentDidMount() {

        const { tracker } = this.props;

        this.setState({ isLoading: true }, async () => {
            try {

                // Call API:
                const result = await CommandService.getConfigs(tracker.id, this.context.user.token);
                const configs = result.data;
                const reminders = (configs["reminder"] ?? '').split(',');

                //console.log(reminders);
                this.setState({
                    isLoading: false,
                    alarms: [
                        this.fromAlarmString(reminders[0]),
                        this.fromAlarmString(reminders[1]),
                        this.fromAlarmString(reminders[2])
                    ]
                });

            } catch (e) {
                this.setState({ isLoading: false });
                showError(e);
            }
        });

    }

    onSelectedChange(index, value) {
        try {

            const alarmsList = this.state.alarms;
            alarmsList[index].selected = value;
            this.setState({
                alarms: alarmsList
            });

        } catch (e) {
            showError(e);
        }
    }

    getAlarmString(alarm) {

        const repeatCode = (!alarm.repeat || alarm.repeat == '0000000') ? 1 : (alarm.repeat == '1111111' ? 2 : 3);
        return `${((alarm.hour ?? 0) + '').padStart(2, '0')}:${((alarm.min ?? 0) + '').padStart(2, '0')}-` +
            `${((alarm.selected ?? false) ? '1' : '0')}-` +
            `${repeatCode}` +
            `${(repeatCode === 3 ? `-${alarm.repeat}` : '')}`;
    }

    fromAlarmString(str) {

        if (!str)
            return {};

        const parts = str.split('-');
        const timeParts = parts[0].split(':');
        const repeatType = Number(parts[2]);
        const repeat = repeatType == 1 ? '0000000' : (repeatType == 2 ? '1111111' : parts[3]);

        return {
            hour: Number(timeParts[0]),
            min: Number(timeParts[1]),
            repeat: repeat,
            selected: parts[1] == '1'
        };
    }

    onSendCommandPress() {
        const { tracker } = this.props;
        this.setState({ error: null, done: false, isSending: true }, async () => {
            try {

                Keyboard.dismiss();

                // Call API:
                const alarmsStr = `${this.getAlarmString(this.state.alarms[0])},` +
                    `${this.getAlarmString(this.state.alarms[1])},` +
                    `${this.getAlarmString(this.state.alarms[2])}`;
                //console.log(alarmsStr);


                const dto = {
                    trackerId: tracker.id,
                    commandType: Commands.REMINDER,
                    payload: alarmsStr
                };
                const result = await CommandService.execute(dto, this.context.user.token, "reminder");

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

    onAlarmPress(index) {
        this.setState({
            editingAlarmIndex: index
        });
    }

    onAlarmEditorConfirmPress(alarm) {
        try {

            const index = this.state.editingAlarmIndex;
            const alarmsList = this.state.alarms;
            alarmsList[index] = { ...alarmsList[index], ...alarm };

            this.setState({
                alarms: alarmsList,
                editingAlarmIndex: -1
            });

        } catch (e) {
            showError(e);
        }
    }

    render() {
        return (
            <View style={styles.container}>
                <BottomSheet isVisible={this.state.editingAlarmIndex >= 0}
                    onClosePress={() => this.setState({ editingAlarmIndex: -1 })}
                >
                    <ReminderEditor
                        alarm={this.state.alarms[this.state.editingAlarmIndex]}
                        onConfirmPress={this.onAlarmEditorConfirmPress}
                    />
                </BottomSheet>

                <ScrollView style={styles.scrollViewer}>
                    <ReminderItem
                        hour={this.state.alarms[0].hour ?? 0}
                        min={this.state.alarms[0].min ?? 0}
                        repeat={this.state.alarms[0].repeat ?? '0000000'}
                        selected={this.state.alarms[0].selected ?? false}
                        onSelectedChange={(v) => {
                            this.onSelectedChange(0, v);
                        }}
                        onPress={() => this.onAlarmPress(0)}
                    />
                    <ReminderItem
                        style={styles.alarmClock}
                        hour={this.state.alarms[1].hour ?? 0}
                        min={this.state.alarms[1].min ?? 0}
                        repeat={this.state.alarms[1].repeat ?? '0000000'}
                        selected={this.state.alarms[1].selected ?? false}
                        onSelectedChange={(v) => {
                            this.onSelectedChange(1, v);
                        }}
                        onPress={() => this.onAlarmPress(1)}
                    />
                    <ReminderItem
                        style={styles.alarmClock}
                        hour={this.state.alarms[2].hour ?? 0}
                        min={this.state.alarms[2].min ?? 0}
                        repeat={this.state.alarms[2].repeat ?? '0000000'}
                        selected={this.state.alarms[2].selected ?? false}
                        onSelectedChange={(v) => {
                            this.onSelectedChange(2, v);
                        }}
                        onPress={() => this.onAlarmPress(2)}
                    />

                    <View style={{ marginTop: vars.PAD_NORMAL }}>
                        {this.state.error && (
                            <FormError error={this.state.error} />
                        )}

                        {this.state.done && (
                            <FormSuccess message={Strings.CommandSentMessage} />
                        )}
                    </View>

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
    alarmClock: {
        marginTop: vars.PAD_NORMAL
    },
    scrollViewer: {
        flex: 1,
        padding: vars.PAD_DOUBLE
    },
    buttonContainer: {
        backgroundColor: vars.COLOR_GRAY_L3,
        padding: vars.PAD_DOUBLE
    },
});