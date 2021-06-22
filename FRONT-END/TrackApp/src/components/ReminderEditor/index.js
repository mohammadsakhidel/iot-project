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

export default function ReminderEditor(props) {

    const {
        alarm,
        onConfirmPress
    } = props;

    const [hour, setHour] = useState(alarm.hour ?? 0);
    const [min, setMin] = useState(alarm.min ?? 0);
    const [repeat, setRepeat] = useState(alarm.repeat ?? '0000000');

    return (
        <View>

            <TimePicker
                hour={hour}
                min={min}
                onChange={(hour, min) => {
                    setHour(hour);
                    setMin(min);
                }}
            />

            <ListItem topDivider onPress={() => { }}>
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
    repeatTitle: {
        fontWeight: 'bold',
        fontSize: vars.FS_BIT_LARGER
    }
});