import React from 'react';
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

const Stack = createStackNavigator();

export default function Entry() {
    return (
        <NavigationContainer >
            <NavigationContext.Provider>
                <Stack.Navigator screenOptions={{
                    headerStyle: GlobalStyles.header,
                    headerTitleStyle: GlobalStyles.headerTitle,
                    headerTintColor: vars.COLOR_GRAY_LIGHTEST
                }}>

                    <Stack.Screen
                        name={RouteNames.HOME_LOGIN_SWITCH}
                        component={HomeLoginSwitch}
                        options={{ headerShown: false }} />

                    <Stack.Screen
                        name={RouteNames.FORGOTTEN_PASSWORD_SCREEN}
                        component={ForgottenPasswordScreen}
                        options={{ headerTitle: Strings.ForgetPassword }} />

                    <Stack.Screen
                        name={RouteNames.ADD_TRACKER}
                        component={TrackerAddScreen}
                        options={{ headerTitle: Strings.AddTrackerTitle }} />

                    <Stack.Screen
                        name={RouteNames.CONFIG_TRACKER}
                        component={TrackerConfigScreen}
                        options={{ headerTitle: Strings.ConfigTrackerTitle }} />

                </Stack.Navigator>
            </NavigationContext.Provider>
        </NavigationContainer>
    );
}