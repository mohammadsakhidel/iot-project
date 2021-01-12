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
    console.error("ERROR -> " + getErrorMessage(error));
    showMessage({
        message: getErrorMessage(error),
        type: 'danger',
        icon: 'danger'
    });
}

export function getErrorMessage(error) {
    return (error instanceof Error ? error.message : error);
}