import React from 'react';
import { Avatar, ListItem } from 'react-native-elements';
import Icon from '../Icon';
import * as vars from '../../styles/vars';
import Text from '../Text';

/**
 * 
 * @param {{icon: String, onPress: Function, chevronShown: Boolean}} props 
 */
export default function SettingsItem(props) {

    const {
        icon,
        onPress,
        children,
        iconColor,
        textColor
    } = props;

    const propSets = {
        chevron: {
            color: vars.COLOR_SECONDARY_L3
        },
        icon: {
            color: iconColor ?? vars.COLOR_SECONDARY_L3
        }
    };

    const chevronShown = props.chevronShown ?? true;

    return (
        <ListItem bottomDivider onPress={onPress}>
            <Icon name={icon} {...propSets.icon} />
            <ListItem.Content>
                <ListItem.Title style={(textColor ? { color: textColor } : {})}>
                    {children}
                </ListItem.Title>
            </ListItem.Content>
            {chevronShown && <ListItem.Chevron {...propSets.chevron} />}
        </ListItem>
    );
}