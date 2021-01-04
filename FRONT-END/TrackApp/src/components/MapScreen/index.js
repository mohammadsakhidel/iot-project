import React, { Component } from 'react';
import { Button, View } from 'react-native';
import Text from '../Text';

export default class MapScreen extends Component {

    constructor(props) {
        super(props);

        // State:
        this.state = {
            message: ''
        };

        // Bindings:
        this.wsConnect = this.wsConnect.bind(this);
        this.wsOnOpen = this.wsOnOpen.bind(this);
        this.wsOnMessage = this.wsOnMessage.bind(this);
        this.wsOnClose = this.wsOnClose.bind(this);
        this.wsOnError = this.wsOnError.bind(this);
        this.onPress = this.onPress.bind(this);

    }

    componentDidMount() {
        //this.wsConnect();
    }

    onPress() {
        if (this.ws)
            this.ws.send("2786b3c0-2182-4c1f-bf78-8e6c23e1ea7a");
    }

    wsConnect() {
        this.setState({ message: "CONNECTING..." });
        this.ws = new WebSocket("ws://192.168.43.95:8125");

        this.ws.onopen = this.wsOnOpen;
        this.ws.onmessage = this.wsOnMessage;
        this.ws.onclose = this.wsOnClose;
        this.ws.onerror = this.wsOnError;
    }

    wsOnOpen(event) {
        this.setState({
            message: "CONNECTION OPENED"
        });
    }

    wsOnMessage(event) {
        this.setState({
            message: event.data
        });
    }

    wsOnClose(event) {
        this.setState({
            message: "CONNECTION CLOSED"
        });

        setTimeout(() => this.wsConnect(), 1000);
    }

    wsOnError(event) {
        this.setState({
            message: "ERROR"
        });
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