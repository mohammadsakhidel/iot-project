import Icon from '../Icon';
import React from 'react';
import { StyleSheet, TouchableOpacity, View } from 'react-native';
import Text from '../Text';
import * as vars from '../../styles/vars';

export default function DrawerMenuItem(props) {

    const {
        iconName,
        title,
        onPress,
        borderBottom
    } = props;

    return (
        <TouchableOpacity onPress={onPress}>
            <View style={[styles.container, {
                borderBottomWidth: borderBottom ? StyleSheet.hairlineWidth : 0
            }]}>
                <Icon
                    name={iconName}
                    color={vars.COLOR_GRAY_L2}
                    size={vars.ICO_BIT_SMALLER}
                    style={styles.icon}
                />
                <Text style={styles.text}>{title}</Text>
            </View>
        </TouchableOpacity>
    );
}

const styles = StyleSheet.create({
    container: {
        flexDirection: 'row',
        alignItems: 'center',
        padding: vars.PAD_HALF,
        borderBottomColor: vars.COLOR_GRAY_L2,
    },
    icon: {
        paddingHorizontal: vars.PAD_NORMAL
    },
    text: {
        padding: vars.PAD_HALF
        //fontSize: vars.FS_BIT_LARGER
    }
});