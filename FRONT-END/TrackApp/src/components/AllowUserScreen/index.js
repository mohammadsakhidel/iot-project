import React, { Component } from 'react';
import { Alert, StyleSheet, View, ScrollView } from 'react-native';
import IconButton from '../IconButton';
import * as vars from '../../styles/vars';
import { Input, Avatar, ListItem } from 'react-native-elements';
import { Strings } from '../../i18n/strings';
import * as GlobalStyles from '../../styles/global-styles';
import SectionHeader from '../SectionHeader';
import PrimaryButton from '../PrimaryButton';
import Text from '../Text';
import { getErrorMessage, showError } from '../FlashMessageWrapper';
import FormError from '../FormError';
import UserService from '../../api/services/user-service';
import AppContext from '../../helpers/app-context';
import LinkButton from '../LinkButton';
import LoadingOver from '../LoadingOver';
import TrackerService from '../../api/services/tracker-service';

export default class AllowUser extends Component {

    static contextType = AppContext;

    constructor(props) {
        super(props);

        // State:
        this.state = {
            userNameOrEmail: null,
            user: null,
            permissions: {},
            isFinding: false,
            findingError: null,
            isSaving: false
        };

        // Bindings:
        this.onOKPress = this.onOKPress.bind(this);
        this.onFindPress = this.onFindPress.bind(this);
        this.onBackToSearchPress = this.onBackToSearchPress.bind(this);

    }

    componentDidMount() {

        // Add Check Action Button:
        const { navigation } = this.props;
        navigation.setOptions({
            headerRight: () => (
                <IconButton
                    name="check"
                    color={vars.COLOR_GRAY_LIGHTEST}
                    onPress={this.onOKPress}
                />
            )
        });

        // Is updating or creating?
        const { tracker, user, permissions } = this.props.route.params;
        if (user && permissions) {
            const permissionsObj = {};
            const rgx = /\s*,\s*/;
            permissions.split(rgx).forEach(p => {
                permissionsObj[p] = true;
            });
            this.setState({
                user: user,
                permissions: permissionsObj
            });
        } else {

            // Focus:
            setTimeout(() => {
                this.input.focus();
            }, 500);

        }
    }

    // METHODS:
    onOKPress() {
        try {
            if (this.state.user == null)
                throw new Error(Strings.ErrorSelectUserFirst);

            this.setState({ isSaving: true }, async () => {
                try {

                    // Arrange:
                    const { tracker } = this.props.route.params;
                    const grantedPermissions = Object.keys(this.state.permissions)
                        .filter(key => this.state.permissions[key] === true);
                    const dto = {
                        trackerId: tracker.id,
                        userId: this.state.user.id,
                        permissions: grantedPermissions.join()
                    };

                    // Call API:
                    const result = await TrackerService.addPermittedUser(dto, this.context.user.token);
                    if (!result.done)
                        throw new Error(result.data);

                    // Update state & Navigate back:
                    this.setState({ isSaving: false }, () => {
                        const { navigation } = this.props;
                        const { onGoBackFunc } = this.props.route.params;
                        onGoBackFunc();
                        navigation.goBack();
                    });

                } catch (e) {
                    showError(e);
                }
            });

        } catch (e) {
            showError(e);
        }
    }

    onFindPress() {
        this.setState({ isFinding: true, findingError: null }, async () => {
            try {

                // Validate:
                if (!this.state.userNameOrEmail) {
                    throw new Error(Strings.ErrorEnterUserEmail);
                }

                // Call API:
                const result = await UserService.find(this.context.user.token, this.state.userNameOrEmail);
                if (!result.done)
                    throw new Error(result.data);
                const user = result.data;

                // Set State's User and Permissions:
                const { tracker } = this.props.route.params;
                const permissions = {};
                tracker.commands.forEach(c => {
                    permissions[c] = true;
                });
                this.setState({
                    user: user,
                    permissions: permissions,
                    isFinding: false
                });

            } catch (e) {
                this.setState({
                    isFinding: false,
                    findingError: getErrorMessage(e)
                });
            }
        });
    }

    onBackToSearchPress() {
        this.setState({
            userNameOrEmail: null,
            user: null,
            permissions: {},
            isFinding: false,
            findingError: null
        });
    }

    onItemSelectionChange(command) {
        let newPermissions = this.state.permissions;
        newPermissions[command] = !newPermissions[command];
        this.setState({
            permissions: newPermissions
        });
    }

    render() {

        const { tracker } = this.props.route.params;

        return (
            <View style={styles.container}>

                {!this.state.user && (
                    <View style={styles.inputContainer}>
                        <Input
                            label={Strings.UsernameOrEmail}
                            ref={(input) => { this.input = input; }}
                            onChangeText={(text) => { this.setState({ userNameOrEmail: text }); }}
                        />
                        <PrimaryButton
                            icon="search"
                            title={Strings.FindUser}
                            onPress={this.onFindPress}
                            isLoading={this.state.isFinding}
                            disabled={this.state.isFinding}
                        />
                        {this.state.findingError && (
                            <View style={styles.findingError}>
                                <FormError error={this.state.findingError} />
                            </View>
                        )}
                    </View>
                )}

                {this.state.user && (
                    <View style={{ flex: 1 }}>

                        <View style={styles.userContainer}>
                            <Avatar
                                rounded
                                size="large"
                                placeholderStyle={{ backgroundColor: vars.COLOR_GRAY_L3 }}
                                source={{
                                    uri: UserService.getAvatarUrl(this.state.user)
                                }}
                                containerStyle={styles.userAvatar}
                            />
                            <View style={styles.userNameContainer}>
                                <Text style={styles.userName}>
                                    {this.state.user ? `${this.state.user.givenName} ${this.state.user.surname}` : ''}
                                </Text>
                                <LinkButton
                                    title={Strings.AnotherUser}
                                    icon="caret-left"
                                    onPress={this.onBackToSearchPress}
                                />
                            </View>
                        </View>

                        <SectionHeader title={Strings.UserPermissions} />

                        <ScrollView style={styles.permissionsContainer}>
                            <View>
                                {tracker.commands.map(command => (
                                    <ListItem key={command} bottomDivider onPress={() => this.onItemSelectionChange(command)}>
                                        <ListItem.CheckBox
                                            checked={this.state.permissions[command]}
                                            onPress={() => this.onItemSelectionChange(command)}
                                            uncheckedColor={vars.COLOR_GRAY_L2}
                                            checkedColor={vars.COLOR_SECONDARY_L3}
                                        />
                                        <ListItem.Content>
                                            <ListItem.Title>{command}</ListItem.Title>
                                        </ListItem.Content>
                                    </ListItem>
                                ))}
                            </View>
                        </ScrollView>

                        <LoadingOver loading={this.state.isSaving} />

                    </View>
                )}

            </View>
        );
    }

}

const styles = StyleSheet.create({
    container: {
        flex: 1
    },
    inputContainer: {
        margin: vars.PAD_DOUBLE
    },
    userContainer: {
        flexDirection: 'row',
        alignItems: 'center',
        padding: vars.PAD_NORMAL
    },
    userAvatar: {
        ...GlobalStyles.marginEndNormal
    },
    userNameContainer: {
        alignItems: 'flex-start'
    },
    userName: {
        fontSize: vars.FS_BIT_LARGER,
        color: vars.COLOR_GRAY,
        fontWeight: 'bold'
    },
    permissionsContainer: {
        flex: 1
    },
    findingError: {
        marginTop: vars.PAD_NORMAL
    }
});