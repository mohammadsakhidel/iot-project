import Icon from '../Icon';
import React from 'react';
import { TouchableOpacity, StyleSheet, Text, View } from 'react-native';
import * as vars from '../../styles/vars';

export default function SmallButton(props) {

    const {
        title,
        buttonStyle,
        iconName,
        ...rest
    } = props;

    return (
        <TouchableOpacity {...rest}>
            <View style={[styles.button, buttonStyle]} >

                <Icon
                    name={iconName}
                    style={styles.icon}
                    fontSize={vars.FS_SMALL}
                />

                <Text style={styles.text}>
                    {title}
                </Text>

            </View>
        </TouchableOpacity>
    );
}

const styles = StyleSheet.create({
    button: {
        flexDirection: 'row',
        alignItems: 'center',
        borderRadius: 8,
        borderWidth: StyleSheet.hairlineWidth,
        borderColor: vars.COLOR_PRIMARY_D1,
        paddingHorizontal: vars.PAD_HALF,
        paddingVertical: 2,
        backgroundColor: vars.COLOR_PRIMARY
    },
    text: {
        color: vars.COLOR_GRAY_LIGHTEST,
        fontSize: vars.FS_BIT_SMALLER,
        marginHorizontal: vars.PAD_HALF
    },
    icon: {
        color: vars.COLOR_GRAY_LIGHTEST,
        fontSize: vars.FS_BIT_SMALLER
    }
});