import { Icon } from 'native-base';
import React from 'react';
import { StyleSheet, Text, TouchableOpacity, View } from 'react-native';
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
                <Icon name={iconName} style={styles.icon} />
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
        color: vars.COLOR_GRAY_L2,
        fontSize: vars.ICO_BIT_SMALLER,
        paddingHorizontal: vars.PAD_BIT_MORE
    },
    text: {
        padding: vars.PAD_HALF,
        fontSize: vars.FS_BIT_LARGER
    }
});