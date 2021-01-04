import React, { Component } from 'react';
import { Button, View } from 'react-native';
import Text from '../Text';

export default class MapScreen extends Component {

    constructor(props) {
        super(props);

        this.ws = new WebSocket("ws://192.168.43.95:8125");
        this.ws.onopen = event => {
            this.setState({
                message: "CONNECTION OPENED"
            });
        };
        this.ws.onmessage = event => {
            this.setState({
                message: event.data
            });
        };
        this.ws.onclose = event => {
            this.setState({
                message: "CONNECTION CLOSED"
            });
        };

        // State:
        this.state = {
            message: ''
        };

        // Bindings:
        this.onPress = this.onPress.bind(this);

    }

    componentDidMount() {
    }

    onPress() {
        this.ws.send("a4c923f8-63a1-4181-a95f-041a218dc8d1");
    }

    render() {
        return (
            <View>
                <Button title="Click To Send" onPress={this.onPress}></Button>
                <Text>Message:</Text>
                <Text>{this.state.message}</Text>
            </View>
        );
    }
};