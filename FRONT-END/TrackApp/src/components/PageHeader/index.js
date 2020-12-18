import React from 'react';
import { StyleSheet } from 'react-native';
import Text from '../Text';
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
        fontFamily: 'TitleFont',
        fontWeight: 'bold',
        color: vars.COLOR_PRIMARY,
        fontSize: vars.FS_LARGE,
        paddingTop: vars.PAD_NORMAL,
        paddingBottom: vars.PAD_DOUBLE
    }
});