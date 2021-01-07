import React from 'react';
import { View, StyleSheet } from 'react-native';
import { QRCode as ReactNativeQRCode } from 'react-native-custom-qr-codes-expo';
import Text from '../Text';

/**
 * 
 * @param {{value: string, size?: number}} props 
 */
export default function QRCode(props) {

    const {
        value,
        size
    } = props;


    return (
        <ReactNativeQRCode
            content={value}
            size={size ?? 200}
        />
    );

}