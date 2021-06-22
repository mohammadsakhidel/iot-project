import React from 'react';
import WheelTimePicker from 'react-native-wheel-time-picker';
import { StyleSheet } from 'react-native';
import * as vars from '../../styles/vars';

export default function TimePicker(props) {

    const { onChange, hour, min } = props;
    
    // Calc default hour min value:
    let defValue = 0;
    if (hour && min) {
        defValue = hour * 60 * 60 * 1000 + min * 60 * 1000;
    }

    return (
        <WheelTimePicker
            value={defValue}
            containerStyle={styles.pickerContainer}
            use24HourSystem={false}
            onChange={newValue => {
                const mins = newValue / 60000;
                const hour = Math.floor(mins / 60);
                const min = Math.floor(mins % 60);

                if (onChange)
                    onChange(hour, min);
            }}
            wheelProps={{
                containerStyle: styles.wheelContainer,
                textStyle: styles.wheelText,
                itemHeight: 40,
                displayCount: 3,
                disabledColor: vars.COLOR_GRAY_L3
            }}
        />
    );
}

const styles = StyleSheet.create({
    pickerContainer: {
        margin: vars.PAD_NORMAL
    },
    wheelText: {
        fontSize: vars.FS_XXLARGE,
        
    },
    wheelContainer: {
        
        width: 50,
        margin: vars.PAD_NORMAL
    }
});