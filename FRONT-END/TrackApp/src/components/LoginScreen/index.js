import React, { useContext } from 'react';
import { Text, View, Button } from 'react-native';
import AppContext from '../../contexts/app-context';

export default function LoginScreen() {

    const appContext = useContext(AppContext);

    const onLogin = () => {
        appContext.setUser({ name: 'mohammada' });
    };

    return (
        <View>
            <Text>Loginnnnn</Text>
            <Button onPress={onLogin} title="Login"></Button>
        </View>
    );
}