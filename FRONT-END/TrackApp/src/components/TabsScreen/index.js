import React from 'react';
import { createBottomTabNavigator } from '@react-navigation/bottom-tabs';
import TrackersScreen from '../TrackersScreen';
import MapScreen from '../MapScreen';
import NotificationsScreen from '../NotificationsScreen';
import MessagesScreen from '../MessagesScreen';

export default function TabsScreen() {

    const Tab = createBottomTabNavigator();
    return (
        <Tab.Navigator>
            <Tab.Screen name='Trackers' component={TrackersScreen} />
            <Tab.Screen name='Map' component={MapScreen} />
            <Tab.Screen name='Notifications' component={NotificationsScreen} />
            <Tab.Screen name='Messages' component={MessagesScreen} />
        </Tab.Navigator>
    );
}