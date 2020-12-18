import Icon from '../Icon';
import React from 'react';
import { TouchableOpacity, StyleSheet, View } from 'react-native';
import Text from '../Text';
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
        padding: vars.PAD_SMALL,
        backgroundColor: vars.COLOR_PRIMARY
    },
    text: {
        color: vars.COLOR_GRAY_LIGHTEST,
        fontSize: vars.FS_BIT_SMALLER,
        marginLeft: vars.PAD_SMALL
    },
    icon: {
        color: vars.COLOR_GRAY_LIGHTEST,
        fontSize: vars.FS_BIT_SMALLER
    }
});