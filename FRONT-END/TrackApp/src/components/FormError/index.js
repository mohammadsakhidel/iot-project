import React from 'react';
import { View, Text } from 'react-native';
import { Icon } from 'native-base';
import * as globalStyles from '../../styles/global-styles';

export default function FormError(props) {

    const { error, ...rest } = props;

    return (
        <View style={{ flexDirection: 'row', alignItems: 'center' }} {...rest}>
            <Icon name="warning" style={{ color: 'red' }} />
            <Text style={[globalStyles.error, { marginHorizontal: 5 }]}>
                {error}
            </Text>
        </View>
    );
}