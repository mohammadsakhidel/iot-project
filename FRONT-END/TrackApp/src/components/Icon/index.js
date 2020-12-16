import React from 'react';
import { Icon as ElementsIcon } from 'react-native-elements';

export default function Icon(props) {

    const {
        type,
        style,
        ...rest
    } = props;

    return (
        <ElementsIcon
            type={(type ?? 'font-awesome')}
            style={style}
            color={style?.color}
            size={style?.fontSize}
            {...rest}
        />
    );

}