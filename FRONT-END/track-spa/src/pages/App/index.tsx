import React from 'react';
import { Typography } from '@material-ui/core';
import { BrowserRouter as Router, Route, Switch } from 'react-router-dom';
import GuardedRoute from '../../components/GuardedRoute';
import * as Routes from '../../constants/route-names';
import MainLayout from '../../layouts/MainLayout';
import Home from '../Home';
import Login from '../Login';
import Panel from '../Panel';


function App() {
  return (
    <Router>
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
    </Router>
  );
}

export default App;