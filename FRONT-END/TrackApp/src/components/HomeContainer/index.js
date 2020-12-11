import React from 'react';
import { StyleSheet } from 'react-native';
import { NavigationContainer } from '@react-navigation/native';
import { createDrawerNavigator } from '@react-navigation/drawer';
import HomeScreen from '../HomeScreen';
import { Strings } from '../../i18n/strings';

const Drawer = createDrawerNavigator();

export default function HomeContainer() {
  return (
    <NavigationContainer>
      <Drawer.Navigator>
        <Drawer.Screen name="HomeScreen" component={HomeScreen} options={{ title: Strings.HomeScreen }} />
      </Drawer.Navigator>
    </NavigationContainer>
  );
}

const styles = StyleSheet.create({
  container: {
    flex: 1
  },
});