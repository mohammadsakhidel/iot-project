import React, { Component } from 'react';
import { View, StyleSheet, Keyboard } from 'react-native';
import Text from '../Text';
import * as vars from '../../styles/vars';
import { Input } from 'react-native-elements';
import { Strings } from '../../i18n/strings';
import PrimaryButton from '../PrimaryButton';
import FormError from '../FormError';
import { getErrorMessage } from '../FlashMessageWrapper';
import CommandService from '../../api/services/command-service';
import AppContext from '../../helpers/app-context';
import FormSuccess from '../FormSuccess';
import * as Patterns from '../../constants/patterns';

export default class AddContactScreen extends Component {

    static contextType = AppContext;

    constructor(props) {
        super(props);

        // State:
        this.state = {
            contactName: null,
            contactNumber: null,
            isLoading: false,
            error: null,
            done: false
        };

        // Bindings:
        this.onAddPress = this.onAddPress.bind(this);

    }

    componentDidMount() {
        setTimeout(() => {
            this.contactNameInput.focus();
        }, 100);
    }

    render() {
        return (
            <View style={styles.container}>

                <Input
                    label={Strings.ContactName}
                    value={this.state.contactName}
                    onChangeText={(text) => this.setState({ contactName: text })}
                    ref={(input) => { this.contactNameInput = input; }}
                />

                <Input
                    label={Strings.PhoneNumber}
                    value={this.state.contactNumber}
                    onChangeText={(text) => this.setState({ contactNumber: text })}
                />

                <PrimaryButton
                    title={Strings.Add}
                    icon="plus"
                    isLoading={this.state.isLoading}
                    disabled={this.state.isLoading}
                    onPress={this.onAddPress}
                />

                <View style={styles.errorContainer}>
                    {this.state.error && (
                        <FormError error={this.state.error} />
                    )}

                    {this.state.done && (
                        <FormSuccess message={Strings.CommandSentMessage} />
                    )}
                </View>

            </View>
        );
    }

    onAddPress() {
        this.setState({ isLoading: true, error: null, done: false }, async () => {
            try {

                Keyboard.dismiss();

                // Validate Inputs:
                if (!this.state.contactName || !this.state.contactNumber)
                    throw new Error(Strings.EnterRequiredFieldsError);

                const rgxPhone = Patterns.PHONE_NUMBER;
                if (!rgxPhone.test(this.state.contactNumber))
                    throw new Error(Strings.InvalidPhoneNumberError);

                const rgxName = Patterns.CONTACT_NAME;
                if (!rgxName.test(this.state.contactName))
                    throw new Error(Strings.InvalidContactNameError);

                // CALL API:
                const { navigation } = this.props;
                const { tracker, onGoBackFunc } = this.props.route.params;
                const dto = {
                    name: this.state.contactName,
                    number: this.state.contactNumber,
                    trackerId: tracker.id
                };
                const result = await CommandService.addContact(dto, this.context.user.token);
                if (!result.done)
                    throw new Error(result.data);

                // Go back:
                onGoBackFunc();
                navigation.goBack();

            } catch (e) {
                this.setState({
                    isLoading: false,
                    error: getErrorMessage(e),
                    done: false
                });
            }
        });
    }
}

const styles = StyleSheet.create({
    container: {
        padding: vars.PAD_DOUBLE
    },
    errorContainer: {
        paddingVertical: vars.PAD_NORMAL
    }
});