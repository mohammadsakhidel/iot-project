import React from 'react';
import { Route, Redirect } from 'react-router-dom';
import * as Routes from '../../constants/routes';

export default function GuardedRoute(props) {

    const {
        component: Component,
        auth,
        ...rest
    } = props;

    return (
        <Route {...rest} render={(props) => (
            auth ? (
                <Component {...props} />
            ) : (
                <Redirect to={Routes.LOGIN} />
            )
        )} />
    );
}