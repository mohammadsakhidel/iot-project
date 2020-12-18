import React from 'react';
import { View, StyleSheet, ScrollView, Keyboard } from 'react-native';
import Text from '../Text';
import AppContext from '../../helpers/app-context';
import AppHeader from '../AppHeader';
import * as vars from '../../styles/vars';
import * as globalStyles from '../../styles/global-styles';
import { Input } from 'react-native-elements';
import PageHeader from '../PageHeader';
import { Strings } from '../../i18n/strings';
import { TouchableWithoutFeedback } from 'react-native-gesture-handler';
import PrimaryButton from '../PrimaryButton';
import FormError from '../FormError';
import { Component } from 'react';
import AuthService from '../../api/services/auth-service';
import LoginDTO from '../../api/dtos/login-dto';
import AppUser from '../../helpers/app-user';
import * as RouteNames from '../../constants/route-names';
import { NavigationContext } from '@react-navigation/native';

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

        this.onLoginPress = this.onLoginPress.bind(this);
        this.onForgotPress = this.onForgotPress.bind(this);
    }

    onLoginPress() {

        // clear isLoading & errors THEN call API and the rest:
        this.setState({ errors: [], isLoading: false }, async () => {
            try {

                Keyboard.dismiss();

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
                const dto = new LoginDTO(this.state.userName, this.state.password);
                const result = await AuthService.login(dto);

                // Process Result:
                if (result.done) {
                    const appUser = AppUser.parseToken(result.data);
                    this.context.login(appUser);
                } else {
                    this.setState({ isLoading: false, errors: [result.data] });
                }

            } catch (e) {
                this.setState({ isLoading: false, errors: [Strings.ErrorMessage, e.message] });
            }
        });

    };

    onForgotPress(navigation) {
        try {
            navigation.navigate(RouteNames.FORGOTTEN_PASSWORD_SCREEN);
        } catch (e) {
            this.setState({ errors: [Strings.ErrorMessage, e.message] });
        }
    }

    render() {
        return (
            <View style={styles.container} >
                <AppHeader />

                <ScrollView style={globalStyles.page} keyboardShouldPersistTaps="always">

                    <PageHeader>
                        {Strings.LoginPageTitle}
                    </PageHeader>

                    <Input
                        placeholder={Strings.UsernameOrEmail}
                        onChangeText={text => this.setState({ userName: text })}
                        defaultValue={this.state.userName} />

                    <Input
                        placeholder={Strings.Password}
                        textContentType="password"
                        secureTextEntry={true}
                        defaultValue={this.state.password}
                        onChangeText={text => this.setState({ password: text })}
                    />

                    <PrimaryButton icon="chevron-right" iconRight
                        disabled={this.state.isLoading} isLoading={this.state.isLoading}
                        style={styles.loginButton} onPress={this.onLoginPress}>
                        {Strings.Login}
                    </PrimaryButton>

                    <NavigationContext.Consumer>
                        {navigation => {
                            return (
                                <TouchableWithoutFeedback onPress={() => this.onForgotPress(navigation)}>
                                    <Text style={[globalStyles.linkButton, styles.forgetButton]}>
                                        {Strings.ForgetPassword}
                                    </Text>
                                </TouchableWithoutFeedback>
                            );
                        }}
                    </NavigationContext.Consumer>

                    <View style={{ marginTop: vars.PAD_BIT_MORE }}>
                        {this.state.errors.map(error => (
                            <FormError key={error} error={error} />
                        ))}
                    </View>

                </ScrollView>

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