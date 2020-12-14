import React from 'react';
import FlashMessage, { showMessage } from 'react-native-flash-message';

export default function FlashMessageWrapper(props) {
    return (
        <FlashMessage
            position="top"
            floating
        />
    );
}

export function showError(error) {
    showMessage({
        message: (error instanceof Error ? error.message : error),
        type: 'danger',
        icon: 'danger'
    });
}