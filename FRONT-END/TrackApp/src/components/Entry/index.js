import React, { Component } from 'react';
import { createStackNavigator } from '@react-navigation/stack';
import { NavigationContainer, NavigationContext } from '@react-navigation/native';
import * as RouteNames from '../../constants/route-names';
import ForgottenPasswordScreen from '../ForgottenPasswordScreen';
import HomeLoginSwitch from '../HomeLoginSwitch';
import { Strings } from '../../i18n/strings';
import TrackerAddScreen from '../TrackerAddScreen';
import TrackerConfigScreen from '../TrackerConfigScreen';
import * as GlobalStyles from '../../styles/global-styles';
import * as vars from '../../styles/vars';
import AppContext from '../../helpers/app-context';
import { connect } from 'react-redux';
import * as Actions from '../../redux/actions';
import * as EventNames from '../../constants/event-names';
import { showError } from '../FlashMessageWrapper';
import EventsService from '../../api/services/events-service';

const Stack = createStackNavigator();

class Entry extends Component {

    static contextType = AppContext;

    constructor(props) {
        super(props);

        // Bindings:
        this.getAccessCodeAndConnect = this.getAccessCodeAndConnect.bind(this);
        this.wsConnect = this.wsConnect.bind(this);
        this.wsOnOpen = this.wsOnOpen.bind(this);
        this.wsOnMessage = this.wsOnMessage.bind(this);
        this.wsOnClose = this.wsOnClose.bind(this);
        this.wsOnError = this.wsOnError.bind(this);
    }

    async componentDidMount() {
        try {
            await this.getAccessCodeAndConnect();
        } catch (e) {
            showError(e);
        }
    }

    render() {
        return (
            <NavigationContainer>
                <NavigationContext.Provider>
                    <Stack.Navigator screenOptions={{
                        headerStyle: GlobalStyles.header,
                        headerTitleStyle: GlobalStyles.headerTitle,
                        headerTintColor: vars.COLOR_GRAY_LIGHTEST
                    }}>

                        <Stack.Screen
                            name={RouteNames.HOME_LOGIN_SWITCH}
                            component={HomeLoginSwitch}
                            options={{ headerShown: false }}
                        />

                        <Stack.Screen
                            name={RouteNames.FORGOTTEN_PASSWORD_SCREEN}
                            component={ForgottenPasswordScreen}
                            options={{ headerTitle: Strings.ForgetPassword }}
                        />

                        <Stack.Screen
                            name={RouteNames.ADD_TRACKER}
                            component={TrackerAddScreen}
                            options={{ headerShown: false }}
                        />

                        <Stack.Screen
                            name={RouteNames.CONFIG_TRACKER}
                            component={TrackerConfigScreen}
                            options={{ headerTitle: Strings.ConfigTrackerTitle }}
                        />

                    </Stack.Navigator>
                </NavigationContext.Provider>
            </NavigationContainer>
        );
    }

    async getAccessCodeAndConnect() {

        // Get Connections Info from API:
        let connectionsInfo = null;
        while (connectionsInfo == null) {
            try {

                const token = this.context.user.token;
                const apiResult = await EventsService.getConnectionInfoAsync(token);
                if (apiResult.done) {

                    connectionsInfo = apiResult.data;

                }

            } catch { }
        }

        // Connect to WebSocket servers:
        const { accessCode, servers } = connectionsInfo;
        servers.forEach(server => {
            this.wsConnect(server[0], server[1], accessCode);
        });

    }

    wsConnect(server, port, accessCode) {
        if (!server || !port || !accessCode)
            return;

        this.accessCode = accessCode;
        this.ws = new WebSocket(`ws://${server}:${port}`);

        this.ws.onopen = this.wsOnOpen;
        this.ws.onmessage = this.wsOnMessage;
        this.ws.onclose = this.wsOnClose;
        this.ws.onerror = this.wsOnError;
    }

    wsOnOpen(e) {
        try {
            if (this.ws) {
                this.ws.send(this.accessCode);
            }
        } catch (e) {
            showError(e);
        }
    }

    wsOnMessage(e) {
        try {
            //console.log(e.data);
            const event = JSON.parse(e.data);

            const {
                changeTrackerStatus
            } = this.props;

            switch (event.name) {
                case EventNames.STATUS_CHANGED:
                    changeTrackerStatus(event);
                    break;
                default:
                    break;
            }
        } catch (e) {
            showError(e);
        }
    }

    wsOnClose(e) {
        setTimeout(async () => {
            await this.getAccessCodeAndConnect()
        }, 1000);
    }

    wsOnError(e) {
        //console.log('WS Connection Error.\n' + JSON.stringify(e));
    }

}

const mapStateToProps = (state) => ({
    trackers: state.trackers,
    connections: state.connections
});

const mapDispatchToProps = (dispatch) => ({
    changeTrackerStatus: (event) => {
        dispatch(Actions.changeTrackerStatus(event))
    }
});

export default connect(mapStateToProps, mapDispatchToProps)(Entry);