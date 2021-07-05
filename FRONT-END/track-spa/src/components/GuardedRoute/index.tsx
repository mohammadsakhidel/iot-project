import React from 'react'
import { Route, Redirect } from 'react-router-dom';
import * as Routes from '../../constants/route-names';

export default function GuardedRoute({ isAuthenticated, children, ...rest } : any) {
    return (
        <Route {...rest}>
            {isAuthenticated ? (
                children
            ) : (
                <Redirect to={Routes.LOGIN} />
            )}
        </Route>
    )
}
