import React from 'react';
import Text from '../Text';
import { StyleSheet, View } from 'react-native';
import * as vars from '../../styles/vars';
import Icon from '../Icon';

export default function ScreenMessage(props) {

    const {
        icon,
        containerStyle,
        textStyle,
        iconStyle,
        children,
        ...rest
    } = props;

    return (
        <View style={[styles.container, containerStyle]}>
            {icon && <Icon name={icon} style={{ ...styles.icon, ...iconStyle }} />}
            <Text {...rest} style={[styles.text, textStyle]}>
                {children}
            </Text>
        </View>
    );
}

const styles = StyleSheet.create({
    container: {
        paddingHorizontal: vars.PAD_DOUBLE,
        marginHorizontal: vars.PAD_NORMAL
    },
    text: {
        color: vars.COLOR_GRAY_L2,
        textAlign: 'center'
    },
    icon: {
        color: vars.COLOR_GRAY_L2
    }
});