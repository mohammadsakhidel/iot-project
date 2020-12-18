import React, { useContext } from 'react';
import { StyleSheet, View, Image, Alert } from "react-native";
import Text from '../Text';
import * as vars from '../../styles/vars';
import { Strings } from '../../i18n/strings';
import SmallButton from '../SmallButton';
import DrawerMenuItem from '../DrawerMenuItem';
import AppContext from '../../helpers/app-context';

export default function DrawerContent(props) {

    const { user } = props;
    const appContext = useContext(AppContext);

    // Funcs:
    const logout = () => {
        try {

            appContext.logout();

        } catch (e) { }
    };
    // ^^^^^

    return (
        <View style={styles.container}>

            {/* Header */}
            <View style={styles.header}>
                <View style={{ flexDirection: 'row', alignItems: 'center' }}>

                    <Image style={styles.profileImage} source={{
                        uri: `https://www.gravatar.com/avatar/${user.emailHash.toLowerCase()}?s=200&d=identicon`
                    }} />

                    <SmallButton
                        iconName="edit"
                        title={Strings.EditProfile}
                        style={{ marginHorizontal: vars.PAD_NORMAL }}
                        buttonStyle={styles.editProfileButton} />

                </View>
                <Text style={styles.userName}>{`${user.givenName} ${user.surname}`}</Text>
            </View>

            {/* Menu */}
            <View style={{ paddingVertical: vars.PAD_HALF }}>
                <DrawerMenuItem
                    iconName="home"
                    title={Strings.WebsiteHome}
                    onPress={() => { }}
                />

                <DrawerMenuItem
                    iconName="envelope"
                    title={Strings.ContactUs}
                    onPress={() => { }}
                    borderBottom={true}
                />

                <DrawerMenuItem
                    iconName="sign-out"
                    title={Strings.Logout}
                    onPress={() => {
                        Alert.alert(
                            Strings.Question,
                            Strings.LogoutSureMessage,
                            [
                                {
                                    text: Strings.Yes,
                                    onPress: () => {
                                        logout();
                                    }
                                },
                                {
                                    text: Strings.Cancel,
                                    onPress: () => { }
                                }
                            ]
                        );
                    }}
                />

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
        marginTop: vars.PAD_BIT_MORE
    },
    profileImage: {
        width: 70,
        height: 70,
        backgroundColor: vars.COLOR_SECONDARY_L1,
        borderRadius: 35,
        borderWidth: StyleSheet.hairlineWidth,
        borderColor: vars.COLOR_GRAY_LIGHTEST
    },
    editProfileButton: {
        backgroundColor: vars.COLOR_SECONDARY_L1,
        borderColor: vars.COLOR_SECONDARY_D1
    }
});