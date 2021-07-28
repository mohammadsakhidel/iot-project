import React, { useState } from 'react';
import { StyleSheet, View } from 'react-native';
import Text from '../Text';
import TimePicker from '../TimePicker';
import * as vars from '../../styles/vars';
import * as globalStyles from '../../styles/global-styles';
import { Strings } from '../../i18n/strings';
import PrimaryButton from '../PrimaryButton';
import { showError } from '../FlashMessageWrapper';

export default function TimePeriodEditor(props) {

    // Props:
    const {
        item,
        onConfirmPress
    } = props;
    const timePeriod = item?.timePeriod;

    // State:
    const [fromHour, setFromHour] = useState(timePeriod?.fromHour ?? 0);
    const [fromMin, setFromMin] = useState(timePeriod?.fromMin ?? 0);
    const [toHour, setToHour] = useState(timePeriod?.toHour ?? 0);
    const [toMin, setToMin] = useState(timePeriod?.toMin ?? 0);

    return (
        <>
            <View style={styles.pickersContainer}>
                <View>
                    <Text style={globalStyles.smallText}>
                        {Strings.From}:
                    </Text>
                    <TimePicker
                        style={styles.timePicker}
                        hour={fromHour}
                        min={fromMin}
                        wheelProps={wheelProps}
                        onChange={(hour, min) => {
                            setFromHour(hour);
                            setFromMin(min);
                        }}
                    />
                </View>

                <View>
                    <Text style={globalStyles.smallText}>
                        {Strings.To}:
                    </Text>
                    <TimePicker
                        style={styles.timePicker}
                        hour={toHour}
                        min={toMin}
                        wheelProps={wheelProps}
                        onChange={(hour, min) => {
                            setToHour(hour);
                            setToMin(min);
                        }}
                    />
                </View>

            </View>

            <PrimaryButton
                icon="check"
                title={Strings.Confirm}
                onPress={() => {
                    // Validate Time Period:
                    const fromValue = fromHour * 60 + fromMin;
                    const toValue = toHour * 60 + toMin;
                    if (toValue <= fromValue) {
                        showError(Strings.InvalidTimePerdiodMessage);
                        return;
                    }

                    if (onConfirmPress)
                        onConfirmPress({
                            fromHour,
                            fromMin,
                            toHour,
                            toMin
                        });
                }}
            />
        </>
    )
}

const styles = StyleSheet.create({
    pickersContainer: {
        flex: 1,
        padding: vars.PAD_NORMAL,
        flexDirection: 'row',
        justifyContent: 'space-around'
    },
    timePicker: {

    }
});

const wheelProps = {
    fontSize: vars.FS_BIT_LARGER,
    width: 35,
    padding: 0,
    margin: 0,
    itemHeight: 20
};