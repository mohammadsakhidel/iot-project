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
import * as AppSettings from '../../appsettings.json';
import PollingService from '../../api/services/polling-service';
import * as EventNames from '../../constants/event-names';
import { connect } from 'react-redux';
import * as Actions from '../../redux/actions';


const Stack = createStackNavigator();

class Entry extends Component {

    static contextType = AppContext;

    constructor(props) {
        super(props);
    }

    componentDidMount() {
        this.pollAsync();
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

    async pollAsync() {

        while (true) {
            try {

                if (this.context.user == null)
                    continue;

                const {
                    trackers,
                    changeTrackerStatus
                } = this.props;
                if (trackers == null || trackers.length == 0)
                    continue;

                const token = this.context.token;
                const pollingInput = {
                    TrackersStatus: trackers.map(t => ({
                        trackerId: t.id,
                        status: t.status ?? ""
                    }))
                };
                console.log(JSON.stringify(pollingInput));
                const response = await PollingService.poll(token, pollingInput);

                if (response.done) {
                    const apiResult = response.data;
                    if (apiResult.done) {
                        const event = JSON.parse(apiResult.data);
                        switch (event.name) {
                            case EventNames.STATUS_CHANGED:
                                changeTrackerStatus(event);
                                break;
                            default:
                                break;
                        }
                    }
                }

            } catch (_) {

            } finally {
                await new Promise(resolve => setTimeout(resolve, AppSettings.PollingDelay ?? 3000));
            }
        }

    }
}

const mapStateToProps = (state) => ({
    trackers: state.trackers
});

const mapDispatchToProps = (dispatch) => ({
    changeTrackerStatus: (event) => {
        dispatch(Actions.changeTrackerStatus(event))
    }
});

export default connect(mapStateToProps, mapDispatchToProps)(Entry);