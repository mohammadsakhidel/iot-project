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
        children
    } = props;

    const chevronShown = props.chevronShown ?? true;

    return (
        <ListItem bottomDivider onPress={onPress}>
            <Icon name={icon} {...propSets.icon} />
            <ListItem.Content>
                <ListItem.Title>
                    {children}
                </ListItem.Title>
            </ListItem.Content>
            {chevronShown && <ListItem.Chevron {...propSets.chevron} />}
        </ListItem>
    );
}

const propSets = {
    chevron: {
        color: vars.COLOR_SECONDARY_L3
    },
    icon: {
        color: vars.COLOR_SECONDARY_L3
    }
};