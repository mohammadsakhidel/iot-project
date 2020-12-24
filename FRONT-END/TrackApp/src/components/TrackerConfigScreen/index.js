import React, { Component } from 'react';
import { View, StyleSheet, ScrollView } from 'react-native';
import Text from '../Text';
import { Avatar, ListItem } from 'react-native-elements';
import Icon from '../Icon';
import TrackerService from '../../api/services/tracker-service';
import * as vars from '../../styles/vars';
import { Strings } from '../../i18n/strings';
import SettingsItem from '../SettingsItem';
import SettingsSection from '../SettingsSection';

export default function TrackerConfigScreen(props) {
    const { route } = props;
    const tracker = route.params;

    return (
        <View style={styles.container}>

            <ListItem bottomDivider onPress={() => { }}>
                <Avatar
                    rounded
                    size="large"
                    source={{ uri: TrackerService.getIconUrl(tracker) }}
                />
                <ListItem.Content>
                    <ListItem.Title>
                        {tracker.displayName}
                    </ListItem.Title>
                </ListItem.Content>
                <ListItem.Chevron
                    name="edit"
                    type="font-awesome"
                    size={vars.ICO_NORMAL}
                    color={vars.COLOR_SECONDARY_L3}
                />
            </ListItem>

            <ScrollView style={styles.settingsContainer}>

                <SettingsSection title={Strings.PrivacySettings}>
                    <SettingsItem icon="qrcode" onPress={() => { }}>
                        {Strings.QRCode}
                    </SettingsItem>
                    <SettingsItem icon="users" onPress={() => { }}>
                        {Strings.AllowedUsers}
                    </SettingsItem>
                    <SettingsItem icon="key" onPress={() => { }}>
                        {Strings.DevicePassword}
                    </SettingsItem>
                </SettingsSection>

                <SettingsSection title={Strings.DeviceSettings}>
                    <SettingsItem icon="mobile" onPress={() => { }}>
                        {Strings.CenterNumber}
                    </SettingsItem>
                    <SettingsItem icon="address-book" onPress={() => { }}>
                        {Strings.Contacts}
                    </SettingsItem>
                    <SettingsItem icon="exclamation-circle" onPress={() => { }}>
                        {Strings.SOSNumbers}
                    </SettingsItem>
                    <SettingsItem icon="bell" onPress={() => { }}>
                        {Strings.AlarmSettings}
                    </SettingsItem>
                    <SettingsItem icon="bell-slash" onPress={() => { }}>
                        {Strings.NoDisturbanceTime}
                    </SettingsItem>
                    <SettingsItem icon="globe" onPress={() => { }}>
                        {Strings.ServerAndPortNumber}
                    </SettingsItem>
                    <SettingsItem icon="history" onPress={() => { }}>
                        {Strings.UploadInterval}
                    </SettingsItem>
                    <SettingsItem icon="language" onPress={() => { }}>
                        {Strings.LanguageAndTimezone}
                    </SettingsItem>
                </SettingsSection>

                <SettingsSection title={Strings.Commands}>
                    <SettingsItem icon="info-circle" onPress={() => { }}>
                        {Strings.CheckVersion}
                    </SettingsItem>
                    <SettingsItem icon="phone" onPress={() => { }}>
                        {Strings.MakeCall}
                    </SettingsItem>
                    <SettingsItem icon="phone-square" onPress={() => { }}>
                        {Strings.FindDevice}
                    </SettingsItem>
                    <SettingsItem icon="bullhorn" onPress={() => { }}>
                        {Strings.Wakeup}
                    </SettingsItem>
                    <SettingsItem icon="repeat" onPress={() => { }}>
                        {Strings.Restart}
                    </SettingsItem>
                    <SettingsItem icon="power-off" onPress={() => { }}>
                        {Strings.PowerOff}
                    </SettingsItem>
                    <SettingsItem icon="minus-circle" onPress={() => { }}>
                        {Strings.ResetFactory}
                    </SettingsItem>
                </SettingsSection>

                <View>
                    <Text style={styles.footer}>
                        {Strings.Footer}
                    </Text>
                </View>

            </ScrollView>

        </View>
    );
}

const styles = StyleSheet.create({
    container: {
        flex: 1
    },
    settingsContainer: {
        flex: 1
    },
    footer: {
        color: vars.COLOR_GRAY_L2,
        padding: vars.PAD_DOUBLE,
        textAlign: 'center',
        fontSize: vars.FS_BIT_LARGER
    }
});