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
import QRCodeScreen from '../QRCodeScreen';
import AllowUserScreen from '../AllowUserScreen';

const Stack = createStackNavigator();

export default class Entry extends Component {

    static contextType = AppContext;

    constructor(props) {
        super(props);

        // State:
        this.state = {
            renderError: null
        };

    }

    render() {

        if (this.state.renderError) {
            return (
                <RenderError error={this.state.renderError} />
            );
        }

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

                        <Stack.Screen
                            name={RouteNames.QR_CODE_SCREEN}
                            component={QRCodeScreen}
                            options={{ headerTitle: Strings.QRCode }}
                        />

                        <Stack.Screen
                            name={RouteNames.ALLOW_USER_SCREEN}
                            component={AllowUserScreen}
                            options={{ headerTitle: Strings.AllowUser }}
                        />

                    </Stack.Navigator>
                </NavigationContext.Provider>
            </NavigationContainer>
        );
    }


}