import React, { Component } from 'react';
import Home from './src/components/Home';
import store from './src/redux/store';
import { Provider } from 'react-redux';
import AppContext from './src/contexts/app-context';

export default class App extends Component {

  constructor(props) {
    super(props);

    this.state = {
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

  render() {
    return (
      <Provider store={store}>
        <AppContext.Provider value={this.state}>
          <Home />
        </AppContext.Provider>
      </Provider>
    );
  }

}