import React, { Component } from 'react';
import { StyleSheet } from 'react-native';
import FlashMessage, { getErrorMessage } from './src/components/FlashMessageWrapper';
import { Ionicons } from '@expo/vector-icons';
import * as Font from 'expo-font';
import AsyncStorage from '@react-native-async-storage/async-storage';
import { AppLoading } from 'expo';
import { View } from 'react-native';
import { Provider } from 'react-redux';
import AppContext from './src/helpers/app-context';
import Store from './src/redux/store';
import { SafeAreaView } from 'react-native-safe-area-context';
import Entry from './src/components/Entry';
import RenderError from './src/components/RenderError';

const customFonts = {
  ContentFont: require('@expo-google-fonts/roboto/Roboto_400Regular.ttf'),
  TitleFont: require('@expo-google-fonts/ubuntu/Ubuntu_400Regular.ttf')
};

export default class App extends Component {

  constructor(props) {
    super(props);

    this.state = {
      isReady: false,
      renderError: null,
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

  async componentDidMount() {
    try {

      await Font.loadAsync({
        ...customFonts,
        ...Ionicons.font,
      });

      await this.loadLoggedInUserAsync();

      setTimeout(() => {
        this.setState({ isReady: true });
      }, 1000);

    } catch (e) {
      this.setState({ error: getErrorMessage(e) });
    }
  }

  render() {

    if (this.state.renderError) {
      return (
        <RenderError error={this.state.renderError} />
      );
    }

    if (!this.state.isReady) {
      return (
        <AppLoading />
      );
    }

    return (
      <Provider store={Store}>
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

  saveUser(appUser) {
    try {

      // Save in storage:
      AsyncStorage.setItem('@user', JSON.stringify(appUser)).then(() => {
        try {

          // Update state:
          this.setState({ user: appUser });

        } catch (e) {
          showError(e);
        }
      });

    } catch (e) {
      this.setState({ error: getErrorMessage(e) });
    }
  }

  removeUser() {
    try {
      if (this.state.user != null) {

        // Remvoe from memory:
        AsyncStorage.removeItem('@user').then(() => {

          try {
            // Updaet state:
            this.setState({ user: null });
          } catch (e) {
            showError(e);
          }

        });

      }
    } catch (e) {
      this.setState({ error: getErrorMessage(e) });
    }
  }

  async loadLoggedInUserAsync() {
    const jsonValue = await AsyncStorage.getItem('@user');
    const appUser = jsonValue != null ? JSON.parse(jsonValue) : null;
    this.setState({ user: appUser });
  }

}

const styles = StyleSheet.create({
  container: {
    flex: 1
  }
});