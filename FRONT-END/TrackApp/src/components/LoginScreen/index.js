import React, { useContext, useState } from 'react';
import { View, StyleSheet } from 'react-native';
import AppContext from '../../contexts/app-context';
import AppHeader from '../AppHeader';
import * as vars from '../../styles/vars';
import * as globalStyles from '../../styles/global-styles';
import { Item, Input, Button, Icon, Text, Spinner } from 'native-base';
import PageHeader from '../PageHeader';
import { Strings } from '../../i18n/strings';
import { TouchableWithoutFeedback } from 'react-native-gesture-handler';
import PrimaryButton from '../PrimaryButton';
import FormError from '../FormError';
import { Component } from 'react';

export default class LoginScreen extends Component {

    static contextType = AppContext;

    constructor(props) {
        super(props);

        // State:
        this.state = {
            isLoading: false,
            errors: [],
            userName: '',
            password: ''
        };

        // Bindings:
        this.onLoginPress = this.onLoginPress.bind(this);
    }

    onLoginPress() {

        // clear isLoading & errors Then call API and the rest:
        this.setState({ errors: [], isLoading: false }, () => {
            try {

                // Validate Inputs:
                const validationErrors = [];
                if (!this.state.userName)
                    validationErrors.push(Strings.ErrorEnterUserName);
                if (!this.state.password)
                    validationErrors.push(Strings.ErrorEnterPassword);
                if (validationErrors.length > 0) {
                    this.setState({ errors: validationErrors });
                    return;
                }

                // Call API:
                this.setState({ isLoading: true });
                setTimeout(() => {
                    this.setState({ isLoading: false, errors: ['Not Implemented!'] });
                }, 2000);

            } catch (e) {
                this.setState({ errors: [Strings.ErrorMessage] });
            }
        });

    };

    render() {
        return (
            <View style={styles.container} >
                <AppHeader />

                <View style={globalStyles.page}>

                    <PageHeader>
                        {Strings.LoginPageTitle}
                    </PageHeader>

                    <Item>
                        <Input placeholder={Strings.UsernameOrEmail}
                            onChangeText={text => this.setState({ userName: text })}
                            defaultValue={this.state.userName} />
                    </Item>
                    <Item>
                        <Input placeholder={Strings.Password}
                            textContentType="password"
                            secureTextEntry={true}
                            defaultValue={this.state.password}
                            onChangeText={text => this.setState({ password: text })}
                        />
                    </Item>

                    <PrimaryButton icon="arrow-forward" iconRight
                        disabled={this.state.isLoading} isLoading={this.state.isLoading}
                        style={styles.loginButton} onPress={this.onLoginPress}>
                        {Strings.Login}
                    </PrimaryButton>

                    <TouchableWithoutFeedback>
                        <Text style={[globalStyles.linkButton, styles.forgetButton]}>
                            {Strings.ForgetPassword}
                        </Text>
                    </TouchableWithoutFeedback>

                    <View style={{ marginTop: vars.PAD_BIT_MORE }}>
                        {this.state.errors.map(error => (
                            <FormError key={error} error={error} />
                        ))}
                    </View>

                </View>

            </View>
        );
    }
}

const styles = StyleSheet.create({
    container: {
        flex: 1
    },
    formContainer: {
        padding: vars.PAD_DOUBLE
    },
    loginButton: {
        marginTop: vars.PAD_BIT_MORE
    },
    forgetButton: {
        marginTop: vars.PAD_BIT_MORE
    }
});