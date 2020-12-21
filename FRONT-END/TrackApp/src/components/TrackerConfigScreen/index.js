import React, { Component } from 'react';
import { View } from 'react-native';
import Text from '../Text';

export default class TrackerConfigScreen extends Component {
    constructor(props) {
        super(props);
    }

    render() {
        return (
            <View>
                <Text>Config Tracker</Text>
            </View>
        );
    }
}