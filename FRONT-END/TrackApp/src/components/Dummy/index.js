import React from 'react';
import { Text } from 'react-native';

export default function Dummy(props) {
    return (
        <Text style={{ color: 'green' }}>{props.children}</Text>
    );
}