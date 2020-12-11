import React, { Component } from 'react';
import * as Font from 'expo-font';
import { AppLoading } from 'expo';
import { Ionicons } from '@expo/vector-icons';
import DrawerLayout from './src/components/DrawerLayout';
import store from './src/redux/store';
import { Provider } from 'react-redux';
import AppContext from './src/contexts/app-context';

export default class App extends Component {

  constructor(props) {
    super(props);

    this.state = {
      isReady: false,
      language: 'en-US',
      theme: 'default',
      setLanguage: lang => this.setState({
        language: lang
      }),
      setTheme: theme => this.setState({
        theme: theme
      })
    };
  }

  async componentDidMount() {
    await Font.loadAsync({
      Roboto: require('native-base/Fonts/Roboto.ttf'),
      Roboto_medium: require('native-base/Fonts/Roboto_medium.ttf'),
      ...Ionicons.font,
    });
    setTimeout(() => {
      this.setState({ isReady: true });
    }, 1000);
  }

  render() {

    if (!this.state.isReady) {
      return <AppLoading />;
    }

    return (
      <Provider store={store}>
        <AppContext.Provider value={this.state}>
          <DrawerLayout />
        </AppContext.Provider>
      </Provider>
    );
  }

}