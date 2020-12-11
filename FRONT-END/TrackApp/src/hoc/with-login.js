import React from 'react';
import LoginScreen from '../components/LoginScreen';

export default function withLogin(Component) {
    return ({ isLoggedIn, ...rest }) => {
        return isLoggedIn
            ? <Component {...rest} />
            : <LoginScreen />
    };
}