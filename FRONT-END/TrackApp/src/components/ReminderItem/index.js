import React from "react";
import { View, StyleSheet } from "react-native";
import * as vars from '../../styles/vars';
import { Switch } from 'react-native-elements';
import Text from '../Text';
import * as globalStyles from '../../styles/global-styles';
import { TouchableHighlight } from "react-native";
import * as TextUtils from '../../utils/text-util';

export default function ReminderItem(props) {

    const {
        hour, min, repeat, selected,
        onPress, onSelectedChange,
        ...rest
    } = props;

    return (
        <TouchableHighlight
            style={[styles.touchable, rest.style]}
            underlayColor={vars.COLOR_SECONDARY_L3}
            onPress={onPress}>
            <View style={styles.container}>
                <View style={styles.timeContainer}>
                    <Text style={styles.time}>
                        {`${hour}`.padStart(2, '0')}:{`${min}`.padStart(2, '0')}
                    </Text>
                    <Text style={[globalStyles.smallText, styles.repetitionText]}>
                        {TextUtils.getRepeatString(repeat)}
                    </Text>
                </View>
                <Switch
                    value={selected}
                    onValueChange={onSelectedChange}
                />
            </View>
        </TouchableHighlight>
    );
}


const styles = StyleSheet.create({
    touchable: {
        borderRadius: 5,
        borderColor: vars.COLOR_GRAY_L1,
        backgroundColor: vars.COLOR_GRAY_LIGHTEST,
        padding: vars.PAD_DOUBLE
    },
    container: {
        flexDirection: 'row'
    },
    time: {
        fontSize: vars.FS_XLARGE,
        fontWeight: "bold"
    },
    timeContainer: {
        flex: 1
    },
    switch: {
        color: vars.COLOR_PRIMARY
    },
    repetitionText: {

    }
});