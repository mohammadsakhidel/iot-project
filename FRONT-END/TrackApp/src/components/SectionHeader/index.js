import React from 'react';
import Text from '../Text';
import { StyleSheet } from 'react-native';
import * as vars from '../../styles/vars';

export default function SectionHeader({ title }) {
    return (
        <Text style={styles.text}>
            {title}
        </Text>
    );
}

const styles = StyleSheet.create({
    text: {
        backgroundColor: vars.COLOR_GRAY_L3,
        color: vars.COLOR_GRAY,
        padding: vars.PAD_SMALL,
        fontSize: vars.FS_BIT_SMALLER,
        fontWeight: 'bold'
    }
});