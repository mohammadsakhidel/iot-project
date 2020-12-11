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

export default function LoginScreen() {

    const [isLoading, setIsLoading] = useState(false);
    const appContext = useContext(AppContext);

    const onLoginPress = () => {
        setIsLoading(true);
        setTimeout(() => {
            //appContext.setUser({ name: 'mohammada' });
            setIsLoading(false);
        }, 2000);
    };

    return (
        <View style={styles.container}>
            <AppHeader />

            <View style={globalStyles.page}>

                <PageHeader>
                    {Strings.LoginPageTitle}
                </PageHeader>

                <Item>
                    <Input placeholder={Strings.UsernameOrEmail} />
                </Item>
                <Item>
                    <Input placeholder={Strings.Password}
                        textContentType="password"
                        secureTextEntry={true}
                    />
                </Item>

                <Button iconRight primary block onPress={onLoginPress} disabled={isLoading}
                    style={[styles.loginButton, globalStyles.primaryButton]}>
                    <Text>{Strings.Login}</Text>
                    <Icon name='arrow-forward' />
                </Button>

                <TouchableWithoutFeedback>
                    <Text style={[globalStyles.linkButton, styles.forgetButton]}>
                        {Strings.ForgetPassword}
                    </Text>
                </TouchableWithoutFeedback>

                {isLoading ? <Spinner color="red" /> : null}

            </View>

        </View>
    );
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