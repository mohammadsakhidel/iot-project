import React from 'react';
import { Component } from 'react';
import { View, Text } from 'react-native';
import PrimaryButton from '../PrimaryButton';
import DateTimePicker from '@react-native-community/datetimepicker';

export default class AlarmEditor extends Component {
    constructor(props) {
        super(props);

        // State:
        this.state = {
            alarm: {}
        };

    }

    render() {

        const {
            onConfirmPress
        } = this.props;

        return (
            <View>
                
                <PrimaryButton
                    title="Confirm"
                    onPress={() => {
                        if (onConfirmPress)
                            onConfirmPress({
                                hour: 13,
                                min: 25,
                                repeat: '0010110'
                            });
                    }}
                />
            </View>
        );
    }
}

