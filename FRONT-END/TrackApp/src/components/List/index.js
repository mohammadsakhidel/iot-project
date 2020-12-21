import React from 'react';
import { StyleSheet, FlatList, View } from 'react-native';
import * as vars from '../../styles/vars';
import Icon from '../Icon';
import Text from '../Text';

/**
 * 
 * @param {{emptyListMessage: string}} props 
 */

export default function List(props) {

    const {
        data,
        emptyListMessage,
        ...rest
    } = props;

    return data && data.length > 0
        ? (
            <FlatList
                data={data}
                {...rest}
            />
        ) : (
            <View style={styles.container}>
                <Icon name="dropbox" style={styles.icon} />
                <Text style={styles.text}>{emptyListMessage}</Text>
            </View>
        );
}

const styles = StyleSheet.create({
    container: {
        flex: 1,
        alignItems: 'center',
        justifyContent: 'center'
    },
    icon: {
        color: vars.COLOR_GRAY_L2
    },
    text: {
        color: vars.COLOR_GRAY_L2,
        paddingHorizontal: vars.PAD_DOUBLE,
        textAlign: 'center',
        marginHorizontal: vars.PAD_NORMAL
    }

});