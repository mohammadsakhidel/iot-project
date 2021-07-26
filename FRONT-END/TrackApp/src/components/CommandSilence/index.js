import React, { Component } from 'react';
import { View, ScrollView, StyleSheet } from 'react-native';
import * as vars from '../../styles/vars';
import * as globalStyles from '../../styles/global-styles';
import TimePeriodItem from '../TimePeriodItem';
import { showError } from '../FlashMessageWrapper';

export default class CommandSilence extends Component {

    constructor(props) {

        super(props);

        // State:
        this.state = {
            items: [
                {
                    selected: true,
                    timePeriod: { fromHour: 7, fromMin: 45, toHour: 14, toMin: 45 }
                }
            ]
        };

        // Bindings: 
        this.onItemSelectionChanged = this.onItemSelectionChanged.bind(this);
        this.onItemPress = this.onItemPress.bind(this);

    }

    onItemSelectionChanged(index, value) {
        try {

            // Change item's selected value:
            if (!this.state.items[index])
                return;
            this.state.items[index].selected = value;

            // Update state:
            const newItems = [...this.state.items];
            this.setState({ items: newItems });

        } catch (e) {
            showError(e);
        }
    }

    onItemPress(index) {
        try {

            console.log(`Item ${index} pressed.`);

        } catch (e) {
            showError(e);
        }
    };

    render() {

        const {
            items
        } = this.state;

        return (
            <ScrollView style={styles.container} >
                <TimePeriodItem
                    {...items[0]}
                    onSelectedChange={(value) => this.onItemSelectionChanged(0, value)}
                    onPress={() => this.onItemPress(0)}
                />
                <TimePeriodItem
                    style={globalStyles.marginTopNormal}
                    {...items[1]}
                    onSelectedChange={(value) => this.onItemSelectionChanged(1, value)}
                    onPress={() => this.onItemPress(1)}
                />
                <TimePeriodItem
                    style={globalStyles.marginTopNormal}
                    {...items[2]}
                    onSelectedChange={(value) => this.onItemSelectionChanged(2, value)}
                    onPress={() => this.onItemPress(2)}
                />
                <TimePeriodItem
                    style={globalStyles.marginTopNormal}
                    {...items[3]}
                    onSelectedChange={(value) => this.onItemSelectionChanged(3, value)}
                    onPress={() => this.onItemPress(3)}
                />
            </ScrollView>
        );
    }
}

const styles = StyleSheet.create({
    container: {
        padding: vars.PAD_NORMAL
    }
});