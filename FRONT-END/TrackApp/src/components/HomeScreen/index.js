import React, { useEffect } from 'react';
import { createBottomTabNavigator } from '@react-navigation/bottom-tabs';
import TrackersScreen from '../TrackersScreen';
import MapScreen from '../MapScreen';
import NotificationsScreen from '../NotificationsScreen';
import MessagesScreen from '../MessagesScreen';
import Icon from '../Icon';
import { StyleSheet, View } from 'react-native';
import Text from '../Text';
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
        fontSize: (focused ? vars.ICO_NORMAL : vars.ICO_SMALL)
    });
    const onHeaderLeftButtonPress = () => {
        props.navigation.openDrawer();
    };

    return (
        <View style={styles.container}>
            <AppHeader
                hasLeft={true}
                hasRight={false}
                leftIconName="menu"
                onLeftPress={onHeaderLeftButtonPress} />

            <Tab.Navigator backBehavior="none"
                tabBarOptions={{
                    style: styles.tabBar,
                    activeTintColor: vars.COLOR_PRIMARY,
                    inactiveTintColor: vars.COLOR_SECONDARY_LIGHTEST
                }}
                sceneContainerStyle={{
                    backgroundColor: vars.COLOR_GRAY_LIGHTEST
                }}>

                <Tab.Screen name='Trackers' component={TrackersScreen} options={{
                    tabBarIcon: ({ focused, color }) => (
                        <Icon name="th" style={getIconStyle(focused, color)} />
                    ),
                    tabBarLabel: ({ focused, color }) => (
                        <Text style={[styles.tabLabel, getLabelStyle(focused, color)]}>
                            {Strings.Trackers}
                        </Text>
                    )
                }} />
                <Tab.Screen name='Map' component={MapScreen} options={{
                    tabBarIcon: ({ focused, color }) => (
                        <Icon name="map-marker" style={getIconStyle(focused, color)} />
                    ),
                    tabBarLabel: ({ focused, color }) => (
                        <Text style={[styles.tabLabel, getLabelStyle(focused, color)]}>
                            {Strings.Map}
                        </Text>
                    )
                }} />
                <Tab.Screen name='Notifications' component={NotificationsScreen} options={{
                    tabBarIcon: ({ focused, color }) => (
                        <Icon name="bell" style={getIconStyle(focused, color)} />
                    ),
                    tabBarLabel: ({ focused, color }) => (
                        <Text style={[styles.tabLabel, getLabelStyle(focused, color)]}>
                            {Strings.Notifications}
                        </Text>
                    )
                }} />
                <Tab.Screen name='Messages' component={MessagesScreen} options={{
                    tabBarIcon: ({ focused, color }) => (
                        <Icon name="envelope" style={getIconStyle(focused, color)} />
                    ),
                    tabBarLabel: ({ focused, color }) => (
                        <Text style={[styles.tabLabel, getLabelStyle(focused, color)]}>
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
        //height: 55,
        paddingBottom: 2,
        backgroundColor: vars.COLOR_SECONDARY,
        borderTopWidth: 5,
        borderTopColor: vars.COLOR_SECONDARY_L1
    },
    tabLabel: {
        fontSize: vars.FS_BIT_SMALLER,
        color: vars.COLOR_GRAY_LIGHTEST
    }
});