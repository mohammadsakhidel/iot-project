import Icon from '../Icon';
import React from 'react';
import { ActivityIndicator, StyleSheet, TouchableOpacity, View } from 'react-native';
import Text from '../Text';
import * as vars from '../../styles/vars';

export default function PrimaryButton(props) {

    const {
        icon,
        children,
        iconRight,
        disabled,
        isLoading,
        ...rest
    } = props;

    const getIcon = () => {
        return (
            <Icon name={icon}
                style={[styles.icon, {
                    display: (isLoading ? 'none' : 'flex')
                }]}
                color={(disabled ? vars.COLOR_GRAY_L1 : vars.COLOR_GRAY_LIGHTEST)}
                size={vars.ICO_BIT_SMALLER}
            />
        );
    };

    return (
        <TouchableOpacity disabled={disabled} {...rest}>
            <View style={[styles.container, {
                backgroundColor: (disabled ? vars.COLOR_GRAY_L3 : vars.COLOR_PRIMARY)
            }]}>

                {(!iconRight ? getIcon() : null)}

                <Text style={[styles.text, {
                    color: (disabled ? vars.COLOR_GRAY_L1 : vars.COLOR_GRAY_LIGHTEST),
                    display: (isLoading ? 'none' : 'flex')
                }]}>{children}</Text>

                {(iconRight ? getIcon() : null)}

                {isLoading
                    ? (
                        <View style={{ flexDirection: 'row' }}>
                            <ActivityIndicator size="small" color={vars.COLOR_GRAY} />
                        </View>
                    )
                    : null
                }

            </View>
        </TouchableOpacity>
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
    }
});