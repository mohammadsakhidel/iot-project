import React from 'react';
import { StyleSheet, Text, View, Image } from "react-native";
import * as vars from '../../styles/vars';
import md5 from 'blueimp-md5';

export default function DrawerContent(props) {

    const { user } = props;
    const emailHash = md5('mohammad.sakhidel@gmail.com');

    return (
        <View style={styles.container}>
            <View style={styles.header}>
                <Image style={styles.profileImage} source={{
                    uri: `https://www.gravatar.com/avatar/${emailHash}?s=200`
                }} />
                <Text style={styles.userName}>{`${user.givenName} ${user.surname}`}</Text>
            </View>
        </View>
    );

}

const styles = StyleSheet.create({
    container: {
        flex: 1
    },
    header: {
        backgroundColor: vars.COLOR_SECONDARY,
        padding: vars.PAD_BIT_MORE
    },
    userName: {
        fontWeight: 'bold',
        color: vars.COLOR_PRIMARY_L2,
        fontSize: vars.FS_BIT_LARGER,
        marginTop: vars.PAD_NORMAL
    },
    profileImage: {
        width: 70,
        height: 70,
        backgroundColor: vars.COLOR_SECONDARY_L1,
        borderRadius: 35,
        borderWidth: StyleSheet.hairlineWidth,
        borderColor: vars.COLOR_GRAY_LIGHTEST
    }
});