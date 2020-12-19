import React from 'react';
import { StyleSheet } from 'react-native';
import { ActivityIndicator } from 'react-native';
import * as vars from '../../styles/vars';

export default function Loading(props) {

    const {
        style,
        color,
        ...rest
    } = props;

    return (
        <ActivityIndicator
            style={[style, styles.loading]}
            color={vars.COLOR_PRIMARY}
            {...rest}
        />
    );
}

const styles = StyleSheet.create({
    loading: {

    }
});
