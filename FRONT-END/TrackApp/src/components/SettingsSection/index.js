import React from 'react';
import { View, StyleSheet } from 'react-native';
import Text from '../Text';
import * as vars from '../../styles/vars';

/**
 * 
 * @param {{title: String}} props 
 */
export default function SettingsSection(props) {

    const {
        title,
        children,
        ...rest
    } = props;

    return (
        <View {...rest}>
            <Text style={styles.title}>
                {title}
            </Text>
            <View>
                {children}
            </View>
        </View>
    );
}

const styles = StyleSheet.create({
    title: {
        color: vars.COLOR_SECONDARY_L1,
        fontWeight: 'bold',
        paddingTop: vars.PAD_NORMAL,
        paddingHorizontal: vars.PAD_NORMAL,
        paddingBottom: vars.PAD_HALF,
    }
});