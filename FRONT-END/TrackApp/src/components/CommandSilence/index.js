import React, { useState } from 'react';
import { View, ScrollView, StyleSheet } from 'react-native';
import * as vars from '../../styles/vars';
import * as globalStyles from '../../styles/global-styles';
import TimePeriodItem from '../TimePeriodItem';
import { showError } from '../FlashMessageWrapper';

export default function CommandSilence(props) {

    // State:
    const [items, setItems] = useState([
        // Item 0:
        {
            selected: true,
            timePeriod: { fromHour: 7, fromMin: 45, toHour: 14, toMin: 45 }
        }
    ]);

    // Functions:
    const onItemSelectionChanged = (index, value) => {
        try {

            // Change item's selected value:
            if (!items[index])
                return;
            items[index].selected = value;

            // Update state:
            setItems({ ...items });

        } catch (e) {
            showError(e);
        }
    };

    const onItemPress = (index) => {
        try {

            console.log(`Item ${index} pressed.`);

        } catch (e) {
            showError(e);
        }
    };

    // Render:
    return (
        <ScrollView style={styles.container}>
            <TimePeriodItem
                {...items[0]}
                onSelectedChange={(value) => onItemSelectionChanged(0, value)}
                onPress={() => onItemPress(0)}
            />
            <TimePeriodItem
                style={globalStyles.marginTopNormal}
                {...items[1]}
                onSelectedChange={(value) => onItemSelectionChanged(1, value)}
                onPress={() => onItemPress(1)}
            />
            <TimePeriodItem
                style={globalStyles.marginTopNormal}
                {...items[2]}
                onSelectedChange={(value) => onItemSelectionChanged(2, value)}
                onPress={() => onItemPress(2)}
            />
            <TimePeriodItem
                style={globalStyles.marginTopNormal}
                {...items[3]}
                onSelectedChange={(value) => onItemSelectionChanged(3, value)}
                onPress={() => onItemPress(3)}
            />
        </ScrollView>
    );
}

const styles = StyleSheet.create({
    container: {
        padding: vars.PAD_NORMAL
    }
});