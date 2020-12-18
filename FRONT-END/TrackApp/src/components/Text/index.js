import React from 'react';
import { Text as ReactNativeText, StyleSheet } from 'react-native';
import * as vars from '../../styles/vars';

export default function Text(props) {

    const {
        children,
        style,
        ...rest
    } = props;

    return (
        <ReactNativeText {...rest} style={[styles.text, style]}>
            {children}
        </ReactNativeText>
    );
}

const styles = StyleSheet.create({
    text: {
        fontSize: vars.FS_NORMAL
    }
});