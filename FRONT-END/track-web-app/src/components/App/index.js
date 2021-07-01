import './index.css';
import { useState } from 'react';
import { Provider } from 'react-redux';
import Store from '../../redux/store';
import AppContext from '../../models/app-context';
import { BrowserRouter as Router, Route, Switch } from 'react-router-dom';
import * as Routes from '../../constants/routes';
import GuardedRoute from '../../hocs/GuardedRoute';
import Home from '../Home';
import Login from '../Login';
import Panel from '../Panel';
import { ThemeProvider } from '@material-ui/core';
import { createMuiTheme } from '@material-ui/core/styles';
import green from '@material-ui/core/colors/green';

const theme = createMuiTheme({
    palette: {
        primary: green,
        
    }
});


function App(props) {

    // State:
    const [state, setState] = useState({
        theme: 'default',
        lang: 'en',
        user: null,
        setTheme: (theme) => {
            setState({ ...state, theme: theme });
        },
        setLang: (lang) => {
            setState({ ...state, lang: lang });
        },
        setUser: (user) => {
            setState({ ...state, user: user });
        }
    });

    return (
        <Router>
            <AppContext.Provider value={state}>
                <Provider store={Store}>
                    <ThemeProvider theme={theme}>
                        <Switch>
                            <Route exact path={Routes.HOME}>
                                <Home></Home>
                            </Route>
                            <Route path={Routes.LOGIN}>
                                <Login></Login>
                            </Route>
                            <GuardedRoute
                                path={Routes.PANEL}
                                component={Panel}
                                auth={false}
                            />
                        </Switch>
                    </ThemeProvider>
                </Provider>
            </AppContext.Provider>
        </Router>
    );
}

export default App;