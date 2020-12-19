import React from 'react';
import { RefreshControl as ReactRefreshControl } from 'react-native';
import * as vars from '../../styles/vars';

export default function RefreshControl(props) {
    return (
        <ReactRefreshControl
            {...props}
            colors={[vars.COLOR_PRIMARY, vars.COLOR_SECONDARY]}
        />
    );
}