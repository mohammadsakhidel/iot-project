import React, { useContext } from 'react';
import HomeContainer from '../HomeContainer';
import LoginScreen from '../LoginScreen';
import AppContext from '../../helpers/app-context';

export default function HomeLoginSwitch() {
    
    const appContext = useContext(AppContext);
    const isLoggedIn = appContext.user != null;
    
    return isLoggedIn ? <HomeContainer /> : <LoginScreen />;

}