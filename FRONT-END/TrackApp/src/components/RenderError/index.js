import React, { useState } from 'react';
import { View, StyleSheet } from 'react-native';
import Text from '../Text';
import Icon from '../Icon';
import * as vars from '../../styles/vars';
import { getErrorMessage } from '../FlashMessageWrapper';

export default function RenderError(props) {

    const { error } = props;

    return (
        <View style={styles.errorContainer}>
            <Icon name="exclamation-circle" style={styles.errorIcon} />
            <Text style={styles.error}>
                {getErrorMessage(error)}
            </Text>
        </View>
    );
}

const styles = StyleSheet.create({
    errorContainer: {
        flex: 1,
        justifyContent: 'center',
        alignItems: 'center',
        padding: vars.PAD_TRIPPLE
    },
    errorIcon: {
        color: vars.COLOR_ERROR
    },
    error: {
        color: vars.COLOR_ERROR,
        fontWeight: 'bold',
        fontSize: vars.FS_BIT_LARGER
    }
});