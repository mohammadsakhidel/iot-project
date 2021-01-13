import React from 'react';
import { View, StyleSheet } from 'react-native';
import Text from '../Text';
import SvgQRCode from 'react-native-qrcode-svg';

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
        <SvgQRCode value={value} size={size} />
    );

}