import React, { Component } from 'react';
import Main from './src/components/Main';
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
          <Main />
        </AppContext.Provider>
      </Provider>
    );
  }

}