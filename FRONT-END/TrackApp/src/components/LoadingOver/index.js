import React from 'react';
import { View, ActivityIndicator, StyleSheet } from 'react-native';
import * as vars from '../../styles/vars';
import Loading from '../Loading';

export default function (props) {

    const { loading } = props;

    return (loading
        ? (
            <View style={styles.container}>
                <View style={styles.back}></View>
                <Loading size="large" />
            </View>
        )
        : null);
}

const styles = StyleSheet.create({
    container: {
        position: 'absolute',
        left: 0,
        right: 0,
        bottom: 0,
        top: 0,
        justifyContent: 'center',
        alignItems: 'center'
    },
    back: {
        position: 'absolute',
        left: 0,
        right: 0,
        bottom: 0,
        top: 0,
        opacity: 0.80,
        backgroundColor: vars.COLOR_GRAY_LIGHTEST
    }
});
