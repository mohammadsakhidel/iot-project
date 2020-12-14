import React, { useContext } from 'react';
import { StyleSheet } from 'react-native';
import { NavigationContainer } from '@react-navigation/native';
import { createDrawerNavigator } from '@react-navigation/drawer';
import HomeScreen from '../HomeScreen';
import { Strings } from '../../i18n/strings';
import DrawerContent from '../DrawerContent';
import AppContext from '../../helpers/app-context';

const Drawer = createDrawerNavigator();

export default function HomeContainer() {

  const appContext = useContext(AppContext);

  return (
    <Drawer.Navigator drawerStyle={styles.drawer}
      drawerContent={() => <DrawerContent user={appContext.user} />}>

      <Drawer.Screen name="HomeScreen" component={HomeScreen} options={{ title: Strings.HomeScreen }} />

    </Drawer.Navigator>
  );
}

const styles = StyleSheet.create({
  container: {
    flex: 1
  },
  drawer: {
  }
});