import React, { Component } from 'react';
import { View } from 'react-native';
import Text from '../Text';

export default class TrackerConfigScreen extends Component {
    constructor(props) {
        super(props);
    }

    render() {

        const { route } = this.props;

        return (
            <View>
                <Text>Config Tracker</Text>
                <Text>{ JSON.stringify(route?.params) }</Text>
            </View>
        );
    }
}