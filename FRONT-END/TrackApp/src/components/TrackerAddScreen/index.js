import React, { Component } from 'react';
import { View } from 'react-native';
import Text from '../Text';

export default class TrackerAddScreen extends Component {
    constructor(props) {
        super(props);
    }

    render() {
        return (
            <View>
                <Text>Add Tracker</Text>
            </View>
        );
    }
}