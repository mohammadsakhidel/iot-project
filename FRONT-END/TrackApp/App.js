import React, { Component } from 'react';
import { Alert, StyleSheet, View } from 'react-native';
import AsyncStorage from '@react-native-async-storage/async-storage';
import * as Font from 'expo-font';
import { AppLoading } from 'expo';
import { Ionicons } from '@expo/vector-icons';
import store from './src/redux/store';
import { Provider } from 'react-redux';
import AppContext from './src/helpers/app-context';
import { SafeAreaView } from 'react-native-safe-area-context';
import Entry from './src/components/Entry';
import FlashMessage, { showError } from './src/components/FlashMessageWrapper';
import { Strings } from './src/i18n/strings';

export default class App extends Component {

  constructor(props) {
    super(props);

    this.state = {
      isReady: false,
      user: null,
      login: (appUser) => {
        this.saveUser(appUser);
      },
      logout: () => {
        this.removeUser();
      }
    };

    // Bindings:
    this.saveUser = this.saveUser.bind(this);
    this.removeUser = this.removeUser.bind(this);
    this.loadLoggedInUserAsync = this.loadLoggedInUserAsync.bind(this);

  }

  saveUser(appUser) {
    try {
      this.setState({ user: appUser }, async () => {
        try {
          await AsyncStorage.setItem('@user', JSON.stringify(appUser));
        } catch (e) {
          showError(e);
        }
      });
    } catch (e) {
      showError(e);
    }
  }

  removeUser() {
    try {
      if (this.state.user != null) {
        this.setState({ user: null }, async () => {
          try {
            await AsyncStorage.removeItem('@user');
          } catch (e) {
            showError(e);
          }
        });
      }
    } catch (e) {
      showError(e);
    }
  }

  async loadLoggedInUserAsync() {
    const jsonValue = await AsyncStorage.getItem('@user');
    const appUser = jsonValue != null ? JSON.parse(jsonValue) : null;
    this.setState({ user: appUser });
  }

  async componentDidMount() {
    try {

      await Font.loadAsync({
        Roboto: require('native-base/Fonts/Roboto.ttf'),
        Roboto_medium: require('native-base/Fonts/Roboto_medium.ttf'),
        ...Ionicons.font,
      });

      await this.loadLoggedInUserAsync();

      setTimeout(() => {
        this.setState({ isReady: true });
      }, 1000);

    } catch (e) {
      showError(e);
    }
  }

  render() {

    if (!this.state.isReady) {
      return <AppLoading />;
    }

    return (
      <Provider store={store}>
        <AppContext.Provider value={this.state}>
          <SafeAreaView style={{ flex: 1 }}>
            <View style={styles.container}>
              <Entry />
              <FlashMessage />
            </View>
          </SafeAreaView>
        </AppContext.Provider>
      </Provider>
    );
  }

}

const styles = StyleSheet.create({
  container: {
    flex: 1
  }
});