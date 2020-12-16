import React from 'react';
import { StyleSheet, Text } from 'react-native';
import * as vars from '../../styles/vars';

export default function PageHeader({ children }) {
    return (
        <Text style={styles.headerText}>
            {children}
        </Text>
    );
}

const styles = StyleSheet.create({
    headerText: {
        fontWeight: 'bold',
        color: vars.COLOR_PRIMARY,
        fontSize: vars.FS_LARGE,
        paddingTop: vars.PAD_NORMAL,
        paddingBottom: vars.PAD_DOUBLE
    }
});