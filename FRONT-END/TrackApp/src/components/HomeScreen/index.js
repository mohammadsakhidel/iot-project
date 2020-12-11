import React, { useEffect } from 'react';
import { createBottomTabNavigator } from '@react-navigation/bottom-tabs';
import TrackersScreen from '../TrackersScreen';
import MapScreen from '../MapScreen';
import NotificationsScreen from '../NotificationsScreen';
import MessagesScreen from '../MessagesScreen';
import { Icon } from 'native-base';
import { StyleSheet, Text, View } from 'react-native';
import { Strings } from '../../i18n/strings';
import * as vars from '../../styles/vars';
import AppHeader from '../AppHeader';

const Tab = createBottomTabNavigator();

export default function HomeScreen(props) {

    // Functions:
    const getLabelStyle = (focused, color) => ({
        color: color,
        display: (focused ? 'flex' : 'none')
    });
    const getIconStyle = (focused, color) => ({
        color: color,
        fontSize: (focused ? 24 : 20)
    });
    const onHeaderLeftButtonPress = () => {
        props.navigation.openDrawer();
    };

    return (
        <View style={styles.container}>
            <AppHeader 
                hasLeft={true} 
                hasRight={true} 
                leftIconName="menu" 
                rightIconName="ellipsis-v" 
                rightIconType="FontAwesome"
                onLeftPress={onHeaderLeftButtonPress} />

            <Tab.Navigator backBehavior="none" tabBarOptions={{ style: styles.tabBar, activeTintColor: vars.COLOR_PRIMARY, inactiveTintColor: vars.COLOR_SECONDARY_LIGHTEST }}>
                <Tab.Screen name='Trackers' component={TrackersScreen} options={{
                    tabBarIcon: ({ focused, color }) => (
                        <Icon name="apps" type="Ionicons" style={{ ...styles.tabIcon, ...getIconStyle(focused, color) }} />
                    ),
                    tabBarLabel: ({ focused, color }) => (
                        <Text style={{ ...styles.tabLabel, ...getLabelStyle(focused, color) }}>
                            {Strings.Trackers}
                        </Text>
                    )
                }} />
                <Tab.Screen name='Map' component={MapScreen} options={{
                    tabBarIcon: ({ focused, color }) => (
                        <Icon name="map-marker" type="FontAwesome" style={{ ...styles.tabIcon, ...getIconStyle(focused, color) }} />
                    ),
                    tabBarLabel: ({ focused, color }) => (
                        <Text style={{ ...styles.tabLabel, ...getLabelStyle(focused, color) }}>
                            {Strings.Map}
                        </Text>
                    )
                }} />
                <Tab.Screen name='Notifications' component={NotificationsScreen} options={{
                    tabBarIcon: ({ focused, color }) => (
                        <Icon name="notifications" type="Ionicons" style={{ ...styles.tabIcon, ...getIconStyle(focused, color) }} />
                    ),
                    tabBarLabel: ({ focused, color }) => (
                        <Text style={{ ...styles.tabLabel, ...getLabelStyle(focused, color) }}>
                            {Strings.Notifications}
                        </Text>
                    )
                }} />
                <Tab.Screen name='Messages' component={MessagesScreen} options={{
                    tabBarIcon: ({ focused, color }) => (
                        <Icon name="mail" type="Ionicons" style={{ ...styles.tabIcon, ...getIconStyle(focused, color) }} />
                    ),
                    tabBarLabel: ({ focused, color }) => (
                        <Text style={{ ...styles.tabLabel, ...getLabelStyle(focused, color) }}>
                            {Strings.Messages}
                        </Text>
                    )
                }} />
            </Tab.Navigator>
            
        </View>
    );

}

const styles = StyleSheet.create({
    container: {
        flex: 1
    },
    tabBar: {
        //height: 60,
        paddingBottom: 5,
        backgroundColor: vars.COLOR_SECONDARY
    },
    tabLabel: {
        fontSize: vars.FS_SMALL
    },
    tabIcon: {

    }
});