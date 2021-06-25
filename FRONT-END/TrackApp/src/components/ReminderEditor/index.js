import React, { useState } from 'react';
import { Component } from 'react';
import { View, Text, StyleSheet } from 'react-native';
import PrimaryButton from '../PrimaryButton';
import TimePicker from '../TimePicker';
import * as vars from '../../styles/vars';
import { Strings } from '../../i18n/strings';
import { ListItem } from 'react-native-elements';
import * as globalStyles from '../../styles/global-styles';
import * as TextUtils from '../../utils/text-util';
import { Overlay } from 'react-native-elements';
import Modal from '../Modal';
import { Alert } from 'react-native';

export default function ReminderEditor(props) {

    const {
        alarm,
        onConfirmPress
    } = props;

    const [hour, setHour] = useState(alarm.hour ?? 0);
    const [min, setMin] = useState(alarm.min ?? 0);
    const [repeat, setRepeat] = useState(alarm.repeat ?? '0000000');
    const [repeatSelection, setRepeatSelection] = useState(repeat);
    const [modalVisible, setModalVisible] = useState(false);

    const toggleRepeatString = (index) => {
        const isChecked = repeatSelection[index] === '1';
        const replacingChar = isChecked ? '0' : '1';
        const newRepeat = `${repeatSelection.slice(0, index)}${replacingChar}${repeatSelection.slice(index + 1, repeatSelection.length)}`;
        setRepeatSelection(newRepeat);
    }

    return (
        <View>
            <Modal
                visible={modalVisible}
                title={Strings.Repeat}
                onBackdropPress={() => setModalVisible(false)}
                onConfirmPress={() => {
                    setRepeat(repeatSelection);
                    setModalVisible(false);
                }}
            >
                {[
                    Strings.Monday, Strings.Tuesday, Strings.Wednesday,
                    Strings.Thursday, Strings.Friday, Strings.Saturday, Strings.Sunday
                ].map((dayname, index) => (
                    <ListItem key={dayname} topDivider={index > 0}
                        onPress={() => toggleRepeatString(index)}>
                        <ListItem.Content>
                            <ListItem.Title>{dayname}</ListItem.Title>
                        </ListItem.Content>
                        <ListItem.CheckBox
                            checked={Number(repeatSelection[index])}
                            onPress={() => toggleRepeatString(index)}
                        />
                    </ListItem>
                ))}
            </Modal>

            <TimePicker
                hour={hour}
                min={min}
                onChange={(hour, min) => {
                    setHour(hour);
                    setMin(min);
                }}
            />

            <ListItem topDivider onPress={() => setModalVisible(true)}>
                <ListItem.Chevron name="edit"></ListItem.Chevron>
                <ListItem.Content>
                    <ListItem.Title style={styles.repeatTitle}>
                        {Strings.Repeat}
                    </ListItem.Title>
                    <ListItem.Subtitle style={globalStyles.smallText}>
                        {TextUtils.getRepeatString(repeat)}
                    </ListItem.Subtitle>
                </ListItem.Content>
            </ListItem>

            <PrimaryButton
                icon="check"
                title={Strings.Confirm}
                onPress={() => {
                    if (onConfirmPress)
                        onConfirmPress({
                            hour: hour,
                            min: min,
                            repeat: repeat
                        });
                }}
            />
        </View>
    );

}

const styles = StyleSheet.create({

});