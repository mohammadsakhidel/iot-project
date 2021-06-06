import React, { useLayoutEffect } from 'react';
import { View, StyleSheet, ScrollView } from 'react-native';
import CommandSingle from '../CommandSingle';
import CommandZero from '../CommandZero';
import CommandGet from '../CommandGet';
import CommandContacts from '../CommandContacts';
import CommandSOS from '../CommandSOS';
import CommandSetServer from '../CommandSetServer';

export default function (props) {

    const { navigation } = props;
    const {
        pageTitle,
        command,
        ...rest
    } = props.route.params;

    useLayoutEffect(() => {
        navigation.setOptions({
            headerTitle: pageTitle
        });
    }, []);

    return (
        <View style={styles.container}>

            {command.type === "single" && (
                <CommandSingle command={command} {...rest} />
            )}

            {command.type === "zero" && (
                <CommandZero command={command} {...rest} />
            )}

            {command.type === "get" && (
                <CommandGet command={command} {...rest} />
            )}

            {command.type === "contacts" && (
                <CommandContacts command={command} {...{ ...rest, navigation }} />
            )}

            {command.type === "sosNumbers" && (
                <CommandSOS command={command} {...rest} />
            )}

            {command.type === "server" && (
                <CommandSetServer command={command} {...rest} />
            )}

        </View>
    );
}

const styles = StyleSheet.create({
    container: {
        flex: 1
    }
});