import React from 'react';
import { View, StyleSheet } from 'react-native';
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
        <Text>QR Code: {value}</Text>
    );

}