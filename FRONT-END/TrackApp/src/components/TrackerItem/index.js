import React from 'react';
import { View } from 'react-native';
import Text from '../Text';

export default function TrackerItem(props) {

    const { item } = props;

    return (
        <View style={{ padding: 10, borderBottomWidth: 1, borderBottomColor: 'gray' }}>
            <Text>{item.id}</Text>
            <Text>{item.displayName}</Text>
        </View>
    );
}