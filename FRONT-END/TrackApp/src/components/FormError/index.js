import React from 'react';
import { View } from 'react-native';
import Text from '../Text';
import Icon from '../Icon';
import * as globalStyles from '../../styles/global-styles';
import * as vars from '../../styles/vars';

export default function FormError(props) {

    const { error, ...rest } = props;

    return (
        <View style={{ flexDirection: 'row', alignItems: 'center', padding: vars.PAD_HALF }} {...rest}>
            <Icon name="minus-circle"
                style={globalStyles.error}
                color={vars.COLOR_ERROR}
                size={vars.ICO_SMALL}
            />
            <Text style={[globalStyles.error, { marginHorizontal: 7, fontWeight: 'bold' }]}>
                {error}
            </Text>
        </View>
    );
}