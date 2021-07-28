import React, { Component } from 'react';
import { View, ScrollView, StyleSheet, Text } from 'react-native';
import * as vars from '../../styles/vars';
import * as globalStyles from '../../styles/global-styles';
import TimePeriodItem from '../TimePeriodItem';
import { showError } from '../FlashMessageWrapper';
import BottomSheet from '../BottomSheet';
import { Strings } from '../../i18n/strings';
import PrimaryButton from '../PrimaryButton';
import TimePeriodEditor from '../TimePeriodEditor';

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
            ],
            editingItemIndex: -1,
            isSending: false
        };

        // Bindings: 
        this.onItemSelectionChanged = this.onItemSelectionChanged.bind(this);
        this.onItemPress = this.onItemPress.bind(this);
        this.onSendCommandPress = this.onSendCommandPress.bind(this);
        this.onTimePeriodEditorConfirm = this.onTimePeriodEditorConfirm.bind(this);
        this.setItem = this.setItem.bind(this);

    }

    onItemSelectionChanged(index, value) {
        try {

            // Change item's selected value:
            const item = this.state.items[index];
            if (!item)
                return;
            item.selected = value;

            // Update state:
            this.setItem(item);

        } catch (e) {
            showError(e);
        }
    }

    onItemPress(index) {
        try {

            this.setState({
                editingItemIndex: index
            });

        } catch (e) {
            showError(e);
        }
    };

    onSendCommandPress() {
        try {

            console.log(this.state.items);

        } catch (e) {
            showError(e);
        }
    }

    onTimePeriodEditorConfirm(timePeriod) {
        try {

            console.log('confirmed.... ' + JSON.stringify(timePeriod));

        } catch (e) {
            showError(e);
        }
    }

    setItem(item, index) {
        const newItems = [...this.state.items];
        newItems[index] = item;

        this.setState({ items: newItems });
    }

    render() {

        const {
            items
        } = this.state;

        return (
            <View style={styles.container}>
                <BottomSheet
                    isVisible={this.state.editingItemIndex >= 0}
                    onClosePress={() => this.setState({ editingItemIndex: -1 })}>

                    <TimePeriodEditor
                        item={this.state.items[this.state.editingItemIndex]}
                        onConfirmPress={this.onTimePeriodEditorConfirm}
                    />

                </BottomSheet>

                <ScrollView style={styles.scrollViewer} >
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

                <View style={styles.buttonContainer}>
                    <PrimaryButton
                        icon="check"
                        title={Strings.SendCommand}
                        onPress={this.onSendCommandPress}
                        isLoading={this.state.isSending}
                        disabled={this.state.isSending}
                    />
                </View>
            </View>
        );
    }
}

const styles = StyleSheet.create({
    container: {
        flex: 1
    },
    item: {
        marginTop: vars.PAD_NORMAL
    },
    scrollViewer: {
        flex: 1,
        padding: vars.PAD_NORMAL
    },
    buttonContainer: {
        backgroundColor: vars.COLOR_GRAY_L3,
        padding: vars.PAD_DOUBLE
    },
});