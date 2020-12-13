import React from 'react';
import { View, Text } from 'react-native';
import { Icon } from 'native-base';
import * as globalStyles from '../../styles/global-styles';

export default function FormError(props) {

    const { error, ...rest } = props;

    return (
        <View style={{ flexDirection: 'row', alignItems: 'center' }} {...rest}>
            <Icon name="remove-circle" style={globalStyles.error} />
            <Text style={[globalStyles.error, { marginHorizontal: 7, fontWeight: 'bold' }]}>
                {error}
            </Text>
        </View>
    );
}