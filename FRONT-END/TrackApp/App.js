import React from 'react';
import Main from './src/components/Main';
import store from './src/redux/store';
import { Provider } from 'react-redux';

export default function App() {
  return (
    <Provider store={store}>
      <Main />
    </Provider>
  );
}