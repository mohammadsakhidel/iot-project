import React from 'react';
import { View, ScrollView, StyleSheet } from 'react-native';
import * as vars from '../../styles/vars';
import * as globalStyles from '../../styles/global-styles';
import TimePeriodItem from '../TimePeriodItem';

export default function CommandSilence(props) {
    return (
        <ScrollView style={styles.container}>
            <TimePeriodItem
                timePeriod={{ fromHour: 7, fromMin: 45, toHour: 14, toMin: 45 }}
                onPress={() => { }}
            />
            <TimePeriodItem
                style={globalStyles.marginTopNormal}
                onPress={() => { }}
            />
            <TimePeriodItem style={globalStyles.marginTopNormal}
                onPress={() => { }}
            />
            <TimePeriodItem style={globalStyles.marginTopNormal}
                onPress={() => { }}
            />
        </ScrollView>
    );
}

const styles = StyleSheet.create({
    container: {
        padding: vars.PAD_NORMAL
    }
});