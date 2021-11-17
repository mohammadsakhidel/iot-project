import Icon from '../Icon';
import React from 'react';
import { StyleSheet } from 'react-native';
import Button from '../Button';
import * as vars from '../../styles/vars';
import PropTypes, { bool } from 'prop-types';

export default function PrimaryButton(props) {

    const {
        icon,
        title,
        disabled,
        isLoading,
        iconRight,
        style,
        iconStyle,
        ...rest
    } = props;

    const getIcon = () => {

        const iconColor = (disabled ? vars.COLOR_GRAY_L1 : vars.COLOR_GRAY_LIGHTEST);

        return (
            <Icon
                name={icon}
                style={{
                    ...styles.icon,
                    ...{
                        display: (isLoading ? 'none' : 'flex'),
                        color: iconColor
                    },
                    ...iconStyle
                }}
                size={vars.ICO_BIT_SMALLER}
            />
        );
    };

    return (
        <Button
            title={title}
            icon={getIcon()}
            loading={isLoading}
            disabled={disabled}
            buttonStyle={{ ...styles.button, ...style }}
            disabledStyle={styles.disabledButton}
            iconRight={iconRight}
            {...rest}
        />
    );
}

const styles = StyleSheet.create({
    container: {
        height: 45,
        flexDirection: 'row',
        justifyContent: 'center',
        alignItems: 'center',
        borderRadius: 3,
        padding: vars.PAD_HALF
    },
    icon: {
        marginHorizontal: 5
    },
    text: {
        fontWeight: 'bold'
    },
    spinner: {
        color: vars.COLOR_GRAY_L1,
        padding: 0,
        margin: 0,
        fontSize: vars.FS_SMALL
    },
    button: {
        backgroundColor: vars.COLOR_PRIMARY
    },
    disabledButton: {
        backgroundColor: vars.COLOR_PRIMARY_D1
    }
});

PrimaryButton.propTypes = {
    icon: PropTypes.string,
    title: PropTypes.string,
    disabled: PropTypes.bool,
    isLoading: PropTypes.bool,
    iconRight: PropTypes.any,
    style: PropTypes.object,
    iconStyle: PropTypes.object
};