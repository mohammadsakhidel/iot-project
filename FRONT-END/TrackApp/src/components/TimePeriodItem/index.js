import React from 'react';
import { View, TouchableHighlight, StyleSheet } from "react-native";
import { Switch } from 'react-native-elements';
import Text from '../Text';
import Icon from '../Icon';
import * as vars from '../../styles/vars';
import * as globalStyles from '../../styles/global-styles';
import { Strings } from '../../i18n/strings';

export default function TimePeriodItem(props) {

    const {
        timePeriod,
        selected,
        onPress,
        onSelectedChange,
        ...rest
    } = props;

    return (
        <TouchableHighlight
            style={[styles.touchable, rest.style]}
            underlayColor={vars.COLOR_SECONDARY_L3}
            onPress={onPress}>

            {timePeriod ? (
                <View style={styles.container}>
                    <View style={styles.timePeriodContainer}>
                        <Text style={styles.timePeriod}>
                            {`${timePeriod.fromHour}`.padStart(2, '0')}:{`${timePeriod.fromMin}`.padStart(2, '0')} - {`${timePeriod.toHour}`.padStart(2, '0')}:{`${timePeriod.toMin}`.padStart(2, '0')}
                        </Text>
                    </View>
                    <Switch
                        value={selected}
                        onValueChange={onSelectedChange}
                    />
                </View>
            ) : (
                <View style={styles.container}>
                    <Icon
                        style={globalStyles.marginEndHalf}
                        name="plus"
                        color={vars.COLOR_PRIMARY}
                        size={vars.ICO_BIT_SMALLER}
                    />

                    <Text>
                        {Strings.AddSilenceTime}
                    </Text>
                </View>
            )}

        </TouchableHighlight>
    );
}

const styles = StyleSheet.create({
    touchable: {
        borderRadius: 3,
        borderColor: vars.COLOR_GRAY_L1,
        backgroundColor: vars.COLOR_GRAY_LIGHTEST,
        padding: vars.PAD_DOUBLE
    },
    container: {
        flexDirection: 'row'
    },
    timePeriod: {
        fontSize: vars.FS_XLARGE,
        fontWeight: "bold"
    },
    timePeriodContainer: {
        flex: 1
    },
});