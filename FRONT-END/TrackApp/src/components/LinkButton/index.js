import React from 'react';
import { StyleSheet } from 'react-native';
import Button from '../Button';
import Icon from '../Icon';
import * as vars from '../../styles/vars';

export default function LinkButton(props) {

    const {
        icon,
        iconStyle,
        titleStyle,
        ...rest
    } = props;

    return (
        <Button
            type="clear"
            titleStyle={[styles.title, titleStyle]}
            icon={(
                <Icon name={icon} style={{ ...styles.icon, ...iconStyle }} />
            )}
            {...rest}
        />
    );
}

const styles = StyleSheet.create({
    title: {
        color: vars.COLOR_LINK,
        fontSize: vars.FS_BIT_SMALLER
    },
    icon: {
        color: vars.COLOR_LINK,
        fontSize: vars.ICO_SMALL,
        marginRight: vars.PAD_SMALL
    }
});