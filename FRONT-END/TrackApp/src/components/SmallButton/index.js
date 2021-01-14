import Icon from '../Icon';
import React from 'react';
import { TouchableOpacity, StyleSheet } from 'react-native';
import Button from '../Button';
import * as vars from '../../styles/vars';
import * as GlobalStyles from '../../styles/global-styles';

export default function SmallButton(props) {

    const {
        title,
        buttonStyle,
        iconName,
        style,
        ...rest
    } = props;

    return (
        <Button
            TouchableComponent={TouchableOpacity}
            title={title}
            buttonStyle={[styles.button, style, buttonStyle]}
            titleStyle={styles.text}
            icon={(
                <Icon name={iconName} style={styles.icon} />
            )}
            {...rest}
        />
    );
}

const styles = StyleSheet.create({
    button: {
        flexDirection: 'row',
        alignItems: 'center',
        borderRadius: 8,
        borderWidth: StyleSheet.hairlineWidth,
        borderColor: vars.COLOR_PRIMARY_D1,
        padding: vars.PAD_SMALL,
        backgroundColor: vars.COLOR_PRIMARY
    },
    text: {
        ...GlobalStyles.marginStartSmall,
        color: vars.COLOR_GRAY_LIGHTEST,
        fontSize: vars.FS_BIT_SMALLER
    },
    icon: {
        color: vars.COLOR_GRAY_LIGHTEST,
        fontSize: vars.FS_BIT_SMALLER
    }
});