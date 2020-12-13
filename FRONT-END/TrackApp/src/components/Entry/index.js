import React from 'react';
import { createStackNavigator } from '@react-navigation/stack';
import { NavigationContainer, NavigationContext } from '@react-navigation/native';
import * as RouteNames from '../../constants/route-names';
import ForgottenPasswordScreen from '../ForgottenPasswordScreen';
import HomeLoginSwitch from '../HomeLoginSwitch';
import { Strings } from '../../i18n/strings';

const Stack = createStackNavigator();

export default function Entry() {
    return (
        <NavigationContainer >
            <NavigationContext.Provider>
                <Stack.Navigator>

                    <Stack.Screen 
                        name={RouteNames.HOME_LOGIN_SWITCH} 
                        component={HomeLoginSwitch} 
                        options={{ headerShown: false }} />

                    <Stack.Screen 
                        name={RouteNames.FORGOTTEN_PASSWORD_SCREEN} 
                        component={ForgottenPasswordScreen}
                        options={{ headerTitle: Strings.ForgetPassword }} />

                </Stack.Navigator>
            </NavigationContext.Provider>
        </NavigationContainer>
    );
}