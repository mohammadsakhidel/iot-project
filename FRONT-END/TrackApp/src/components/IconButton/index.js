import React from 'react';
import { TouchableOpacity } from 'react-native';
import { Button } from 'react-native-elements';
import Icon from '../Icon';

export default function IconButton({ name, size, color, ...rest }) {
    return (
        <Button
            type="clear"
            icon={{
                name: name,
                color: color,
                size: size,
                type: "font-awesome"
            }}
            titleStyle={{ color: color }}
            disabledStyle={{ opacity: 0.5 }}
            {...rest}
        />
    );
}