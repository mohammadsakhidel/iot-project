import React, { Component } from "react";
import { View, StyleSheet } from "react-native";
import * as vars from '../../styles/vars';
import { Switch } from 'react-native-elements';
import Text from '../Text';
import * as globalStyles from '../../styles/global-styles';
import { Strings } from '../../i18n/strings';
import { TouchableOpacity } from "react-native";
import { TouchableHighlight } from "react-native";

export default function AlarmClock(props) {

    const {
        hour, min, repeat, selected,
        onPress, onSelectedChange,
        ...rest
    } = props;

    const repetitionStringToDayNames = (repStr) => {
        const array = [];
        if (!repStr || repStr.length < 7)
            return '';

        if (repStr.charAt(0) == '1')
            array.push(Strings.Monday);
        if (repStr.charAt(1) == '1')
            array.push(Strings.Tuesday);
        if (repStr.charAt(2) == '1')
            array.push(Strings.Wednesday);
        if (repStr.charAt(3) == '1')
            array.push(Strings.Thursday);
        if (repStr.charAt(4) == '1')
            array.push(Strings.Friday);
        if (repStr.charAt(5) == '1')
            array.push(Strings.Saturday);
        if (repStr.charAt(6) == '1')
            array.push(Strings.Sunday);

        return array.join(', ');
    };

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
                        {
                            (!repeat || repeat == '0000000') ? Strings.Once : (repeat == '1111111' ? Strings.EveryDay : repetitionStringToDayNames(repeat))
                        }
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