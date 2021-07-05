import React from 'react';
import { Typography } from '@material-ui/core';
import { createMuiTheme, ThemeProvider } from '@material-ui/core/styles';
import { BrowserRouter as Router, Route, Switch } from 'react-router-dom';
import GuardedRoute from '../../components/GuardedRoute';
import * as Routes from '../../constants/route-names';
import MainLayout from '../../layouts/MainLayout';
import Home from '../Home';
import Login from '../Login';
import Panel from '../Panel';
import * as Vars from '../../styles/vars';

const theme = createMuiTheme({
  palette: {
    secondary: {
      main: Vars.COLOR_PRIMARY,
      light: Vars.COLOR_PRIMARY_L2,
      dark: Vars.COLOR_PRIMARY_D1
    },
    primary: {
      main: Vars.COLOR_SECONDARY,
      light: Vars.COLOR_SECONDARY_L2,
      dark: Vars.COLOR_SECONDARY_D1
    }
  },
  mixins: {
    toolbar: {
      minHeight: 70
    }
  },
  typography: {
    fontFamily: "Quicksand, Roboto, Arial, sans-serif"
  }
});

function App() {
  return (
    <Router>
      <ThemeProvider theme={theme}>
        <MainLayout>
          <Switch>
            <Route exact path={Routes.HOME}>
              <Home />
            </Route>
            <Route path={Routes.LOGIN}>
              <Login />
            </Route>
            <GuardedRoute path={Routes.PANEL} isAuthenticated={true}>
              <Panel />
            </GuardedRoute>
          </Switch>
        </MainLayout>
      </ThemeProvider>
    </Router>
  );
}

export default App;