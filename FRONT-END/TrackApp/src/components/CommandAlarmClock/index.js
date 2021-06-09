import React, { Component } from "react";
import { View, ScrollView, StyleSheet, Alarm } from "react-native";
import * as vars from '../../styles/vars';
import AlarmClock from "../AlarmClock";
import { showError } from '../FlashMessageWrapper';
import PrimaryButton from '../PrimaryButton';
import { Strings } from '../../i18n/strings';
import BottomSheet from '../BottomSheet';
import AlarmEditor from '../AlarmEditor';

export default class CommandAlarmClock extends Component {
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

    onSendCommandPress() {

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
            alarmsList[index] = {...alarmsList[index], ...alarm};

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
                    <AlarmEditor onConfirmPress={this.onAlarmEditorConfirmPress} />
                </BottomSheet>

                <ScrollView style={styles.scrollViewer}>
                    <AlarmClock
                        hour={this.state.alarms[0].hour ?? 0}
                        min={this.state.alarms[0].min ?? 0}
                        repeat={this.state.alarms[0].repeat ?? '0000000'}
                        selected={this.state.alarms[0].selected ?? false}
                        onSelectedChange={(v) => {
                            this.onSelectedChange(0, v);
                        }}
                        onPress={() => this.onAlarmPress(0)}
                    />
                    <AlarmClock
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
                    <AlarmClock
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