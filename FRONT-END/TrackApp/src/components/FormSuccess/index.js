import React from 'react';
import { View, StyleSheet } from 'react-native';
import * as globalStyles from '../../styles/global-styles';
import * as vars from '../../styles/vars';
import Text from '../Text';
import Icon from '../Icon';

export default function FormSuccess(props) {

    const { message, ...rest } = props;

    return (
        <View style={styles.container} {...rest}>
            <Icon name="check-circle"
                style={globalStyles.success}
                color={vars.COLOR_SUCCESS}
                size={vars.ICO_SMALL}
            />
            <Text style={[globalStyles.success, styles.text]}>
                {message}
            </Text>
        </View>
    );
}

const styles = StyleSheet.create({
    container: {
        flexDirection: 'row',
        alignItems: 'center',
        padding: vars.PAD_HALF
    },
    text: {
        marginHorizontal: 7,
        fontWeight: 'bold'
    }
});