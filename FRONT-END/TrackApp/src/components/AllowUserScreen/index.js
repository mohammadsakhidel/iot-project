import React, { useLayoutEffect } from 'react';
import { Alert, View } from 'react-native';
import Button from '../Button';
import Text from '../Text';
import IconButton from '../IconButton';
import * as vars from '../../styles/vars';

export default function AllowUser({ navigation, route }) {

    // VARS:
    const tracker = route.params;

    // HOOKS:
    useLayoutEffect(() => {
        navigation.setOptions({
            headerRight: () => (
                <IconButton
                    name="check"
                    color={vars.COLOR_GRAY_LIGHTEST}
                    onPress={onAddPress}
                />
            )
        });
    }, [navigation]);

    // METHODS:
    const onAddPress = () => {
        Alert.alert('test add...');
    };

    // RENDER:
    return (
        <View>
            <Text>Allow User Screen</Text>
            <Text>{tracker.displayName}</Text>
        </View>
    );

}