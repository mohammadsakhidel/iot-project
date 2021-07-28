import React, { Component } from 'react';
import { View, ScrollView, StyleSheet, Keyboard } from 'react-native';
import * as vars from '../../styles/vars';
import * as globalStyles from '../../styles/global-styles';
import TimePeriodItem from '../TimePeriodItem';
import { showError, getErrorMessage } from '../FlashMessageWrapper';
import BottomSheet from '../BottomSheet';
import { Strings } from '../../i18n/strings';
import PrimaryButton from '../PrimaryButton';
import TimePeriodEditor from '../TimePeriodEditor';
import * as Commands from '../../constants/command-names';
import AppContext from '../../helpers/app-context';
import CommandService from '../../api/services/command-service';
import * as ErrorCodes from '../../constants/error-codes';
import FormError from '../FormError';
import FormSuccess from '../FormSuccess';

export default class CommandSilence extends Component {

    static contextType = AppContext;

    constructor(props) {

        super(props);

        // State:
        this.state = {
            items: [],
            editingItemIndex: -1,
            isSending: false,
            isLoading: false,
            error: null,
            done: false
        };

        // Bindings: 
        this.onItemSelectionChanged = this.onItemSelectionChanged.bind(this);
        this.onItemPress = this.onItemPress.bind(this);
        this.onSendCommandPress = this.onSendCommandPress.bind(this);
        this.onTimePeriodEditorConfirm = this.onTimePeriodEditorConfirm.bind(this);
        this.setItem = this.setItem.bind(this);
        this.toTimePeriodString = this.toTimePeriodString.bind(this);

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
        const { tracker } = this.props;
        this.setState({ error: null, done: false, isSending: true }, async () => {
            try {

                Keyboard.dismiss();

                // Call API:
                const itemsStr = `${this.toTimePeriodString(this.state.items[0]?.selected ? this.state.items[0]?.timePeriod : null)},` +
                    `${this.toTimePeriodString(this.state.items[1]?.selected ? this.state.items[1]?.timePeriod : null)},` +
                    `${this.toTimePeriodString(this.state.items[2]?.selected ? this.state.items[2]?.timePeriod : null)},` +
                    `${this.toTimePeriodString(this.state.items[3]?.selected ? this.state.items[3]?.timePeriod : null)}`;

                const dto = {
                    trackerId: tracker.id,
                    commandType: Commands.NO_DISTURBANCE,
                    payload: itemsStr
                };
                const result = await CommandService.execute(dto, this.context.user.token, "silence");

                // Show Result:
                if (result.done) {
                    this.setState({ isSending: false, done: true });
                } else {
                    let errorMessage = "";
                    switch (result.data) {
                        case ErrorCodes.TRACKER_OFFLINE:
                            errorMessage = Strings.ErrorCodeTrackerOffline
                            break;
                        default:
                            errorMessage = result.data;
                            break;
                    }
                    this.setState({ isSending: false, done: false, error: errorMessage });
                }

            } catch (e) {
                this.setState({
                    isSending: false,
                    done: false,
                    error: getErrorMessage(e)
                });
            }
        });
    }

    onTimePeriodEditorConfirm(timePeriod) {
        try {

            let item = { ...(this.state.items[this.state.editingItemIndex] ?? { selected: true }) };
            item.timePeriod = timePeriod;

            this.setItem(item, this.state.editingItemIndex);
            this.setState({
                editingItemIndex: -1
            });

        } catch (e) {
            showError(e);
        }
    }

    setItem(item, index) {
        const newItems = [...this.state.items];
        newItems[index] = item;

        this.setState({ items: newItems });
    }

    toTimePeriodString(timePeriod) {
        return `${((timePeriod?.fromHour ?? 0) + '').padStart(2, '0')}` +
            `:${((timePeriod?.fromMin ?? 0) + '').padStart(2, '0')}` +
            `-${((timePeriod?.toHour ?? 0) + '').padStart(2, '0')}` +
            `:${((timePeriod?.toMin ?? 0) + '').padStart(2, '0')}`;
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

                    <View style={{ marginTop: vars.PAD_NORMAL }}>
                        {this.state.error && (
                            <FormError error={this.state.error} />
                        )}

                        {this.state.done && (
                            <FormSuccess message={Strings.CommandSentMessage} />
                        )}
                    </View>
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