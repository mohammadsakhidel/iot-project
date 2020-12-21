import React from 'react';
import { StyleSheet, TouchableOpacity, View } from 'react-native';
import * as vars from '../../styles/vars';
import Icon from '../Icon';

export default function FloatingButton(props) {

    const {
        buttonStyle,
        icon,
        onPress
    } = props;

    return (
        <TouchableOpacity
            style={styles.buttonContainer}
            onPress={onPress}>

            <View style={[styles.button, buttonStyle]}>
                <Icon name={icon} style={styles.icon} />
            </View>

        </TouchableOpacity>
    );
}

const styles = StyleSheet.create({
    buttonContainer: {
        position: 'absolute',
        right: vars.PAD_DOUBLE,
        bottom: vars.PAD_DOUBLE
    },
    button: {
        width: 60,
        height: 60,
        borderRadius: 30,
        alignItems: 'center',
        justifyContent: 'center',
        backgroundColor: vars.COLOR_PRIMARY,
    },
    icon: {
        color: vars.COLOR_GRAY_LIGHTEST,
        fontSize: vars.ICO_SMALL
    }
});