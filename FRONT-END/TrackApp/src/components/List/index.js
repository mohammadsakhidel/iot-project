import React from 'react';
import { StyleSheet, FlatList, View } from 'react-native';
import * as vars from '../../styles/vars';
import Icon from '../Icon';
import LinkButton from '../LinkButton';
import Text from '../Text';
import { Strings } from '../../i18n/strings';

/**
 * 
 * @param {{emptyListMessage: string}} props 
 */

export default function List(props) {

    const {
        data,
        emptyListMessage,
        reloadFunc,
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
                <Icon name="dropbox" style={styles.icon} size={vars.ICO_LARGE} />
                <Text style={styles.text}>{emptyListMessage}</Text>
                {reloadFunc && <LinkButton title={Strings.Reload} icon="refresh" onPress={reloadFunc} />}
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