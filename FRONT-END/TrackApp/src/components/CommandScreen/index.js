import React, { useLayoutEffect } from 'react';
import { View, StyleSheet, ScrollView } from 'react-native';
import CommandSingle from '../CommandSingle';


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

        </View>
    );
}

const styles = StyleSheet.create({
    container: {
        flex: 1
    }
});