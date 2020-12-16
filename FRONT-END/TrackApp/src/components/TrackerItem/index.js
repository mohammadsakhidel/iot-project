import React from 'react';
import { Text, View } from 'react-native';

export default function TrackerItem(props) {

    const { item } = props;

    return (
        <View style={{ padding: 10, borderBottomWidth: 1, borderBottomColor: 'gray' }}>
            <Text>{item.id}</Text>
            <Text>{item.displayName}</Text>
        </View>
    );
}