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

export default class AllowUser extends Component {//({ navigation, route }) {

    constructor(props) {
        super(props);

        // State:
        this.state = {
            user: null,
            permissions: {}
        };

        // Bindings:
        this.onOKPress = this.onOKPress.bind(this);
        this.onFindPress = this.onFindPress.bind(this);

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

        // Focus:
        setTimeout(() => {
            this.input.focus();
        }, 500);


    }

    // METHODS:
    onOKPress() {
        Alert.alert('test add...');
    }

    onFindPress() {
        const tracker = this.props.route?.params;

        const defaultPermissions = {};
        tracker.commands.forEach(c => {
            defaultPermissions[c] = true;
        });

        this.setState({
            user: { name: "Mohammad Sakhidel" },
            permissions: defaultPermissions
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

        const tracker = this.props.route?.params;

        return (
            <View style={styles.container}>

                {!this.state.user && (
                    <View style={styles.inputContainer}>
                        <Input
                            label={Strings.UsernameOrEmail}
                            ref={(input) => { this.input = input; }}
                        />
                        <PrimaryButton icon="search" title={Strings.FindUser} onPress={this.onFindPress} />
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
                                    uri: "https://scholarshipscorner.website/wp-content/uploads/2019/09/stencil-2019-09-16T135948.210.png"
                                }}
                                containerStyle={styles.userAvatar}
                            />
                            <Text style={styles.userName}>
                                Mohammad Sakhidel
                            </Text>
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
    userName: {
        fontSize: vars.FS_BIT_LARGER,
        color: vars.COLOR_GRAY,
        fontWeight: 'bold'
    },
    permissionsContainer: {
        flex: 1
    }
});