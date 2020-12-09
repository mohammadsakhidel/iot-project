import React from 'react';
import { StyleSheet, SafeAreaView, Platform, StatusBar, View, Text } from 'react-native';
import { NavigationContainer } from '@react-navigation/native';
import { createBottomTabNavigator } from '@react-navigation/bottom-tabs';
import HomeScreen from '../HomeScreen';
import MapScreen from '../MapScreen';
import NotificationsScreen from '../NotificationsScreen';
import MessagesScreen from '../MessagesScreen';

const Tab = createBottomTabNavigator();

export default function Main() {
  return (
    <NavigationContainer>
      <SafeAreaView style={{ flex: 1, ...styles.container }}>
        <Tab.Navigator>
          <Tab.Screen name='Home' component={HomeScreen} />
          <Tab.Screen name='Map' component={MapScreen} />
          <Tab.Screen name='Notifications' component={NotificationsScreen} />
          <Tab.Screen name='Messages' component={MessagesScreen} />
        </Tab.Navigator>
      </SafeAreaView>
    </NavigationContainer>
  );
}

const styles = StyleSheet.create({
  container: {
    flex: 1,
    // Since SafeAreaView only works on IOS we need to add padding top on android:
    paddingTop: Platform.OS === "android" ? StatusBar.currentHeight : 0
  },
});