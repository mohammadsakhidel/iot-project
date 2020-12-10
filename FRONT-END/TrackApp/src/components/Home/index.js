import React from 'react';
import { StyleSheet } from 'react-native';
import { NavigationContainer } from '@react-navigation/native';
import { createDrawerNavigator } from '@react-navigation/drawer';
import { SafeAreaView } from 'react-native-safe-area-context';
import TabsScreen from '../TabsScreen';
import Strings from '../../i18n/strings';

const Drawer = createDrawerNavigator();

export default function Home() {
  return (
    <NavigationContainer>
      <SafeAreaView style={styles.container}>
        <Drawer.Navigator>
          <Drawer.Screen name="Tabs" component={TabsScreen} options={{ title: Strings.get('homePage') }} />
        </Drawer.Navigator>
      </SafeAreaView>
    </NavigationContainer>
  );
}

const styles = StyleSheet.create({
  container: {
    flex: 1
  },
});