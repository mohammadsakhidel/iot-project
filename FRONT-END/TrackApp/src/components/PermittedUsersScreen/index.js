import React, { Component } from 'react';
import { StyleSheet, View, Alert } from 'react-native';
import Loading from '../Loading';
import Text from '../Text';
import * as RouteNames from '../../constants/route-names';
import FloatingButton from '../FloatingButton';
import List from '../List';
import { Strings } from '../../i18n/strings';
import RefreshControl from '../RefreshControl';
import TrackerService from '../../api/services/tracker-service';
import AppContext from '../../helpers/app-context';
import { showError } from '../FlashMessageWrapper';
import { ListItem } from 'react-native-elements';
import { Avatar } from 'react-native-elements';
import * as vars from '../../styles/vars';
import UserService from '../../api/services/user-service';

export default class PermittedUsersScreen extends Component {

    static contextType = AppContext;

    constructor(props) {
        super(props);

        // State:
        this.state = {
            isLoading: false,
            isRefreshing: false,
            items: []
        };

        // Bindings:
        this.loadItems = this.loadItems.bind(this);
        this.onRefresh = this.onRefresh.bind(this);
        this.onRemovePermittedUserPress = this.onRemovePermittedUserPress.bind(this);
        this.removePermittedUser = this.removePermittedUser.bind(this);
        this.onListItemPress = this.onListItemPress.bind(this);

    }

    componentDidMount() {
        this.loadItems();
    }

    loadItems() {
        this.setState({ isLoading: true }, async () => {
            try {

                const { tracker } = this.props.route.params;
                const result = await TrackerService.getPermittedUsers(tracker.id, this.context.user.token);

                if (!result.done)
                    throw new Error(result.data);

                this.setState({
                    isLoading: false,
                    isRefreshing: false,
                    items: result.data
                });

            } catch (e) {
                this.setState({ isLoading: false });
                showError(e);
            }
        });
    }

    onRefresh() {
        this.setState({ isRefreshing: true }, () => {
            this.loadItems();
        });
    }

    onRemovePermittedUserPress(item) {
        try {

            Alert.alert(
                Strings.AreYouSure,
                Strings.AreYouSureRemoveItem,
                [
                    {
                        text: Strings.Yes,
                        onPress: () => this.removePermittedUser(item)
                    },
                    {
                        text: Strings.Cancel,
                        onPress: () => { }
                    }
                ]
            );

        } catch (e) {
            showError(e);
        }
    }

    removePermittedUser(item) {
        this.setState({ isLoading: true }, async () => {
            try {

                const { tracker } = this.props.route.params;
                const { user } = item;

                const result = await TrackerService.removePermittedUser(tracker.id, user.id);
                if (result.done) {
                    this.loadItems();
                }

            } catch (e) {
                this.setState({ isLoading: false });
                showError(e);
            }
        });
    }

    onListItemPress(item) {

        const { navigation } = this.props;
        const { tracker } = this.props.route.params;
        const { user, permissions } = item;

        navigation.navigate(RouteNames.ALLOW_USER_SCREEN, {
            tracker,
            user,
            permissions,
            onGoBackFunc: this.loadItems
        });


    }

    render() {

        const { tracker } = this.props.route.params;
        const { navigation } = this.props;

        return (
            <View style={styles.container}>
                {this.state.isLoading
                    ? (
                        <View style={styles.loadingContainer}>
                            <Loading size="large" />
                        </View>
                    )
                    : (
                        <View style={{ flex: 1 }}>
                            <List
                                emptyListMessage={Strings.EmptyListMessage}
                                reloadFunc={this.loadItems}
                                data={this.state.items}
                                keyExtractor={(item, index) => item.user.id}
                                renderItem={({ item }) => (
                                    <ListItem key={item.user.id} bottomDivider onPress={() => this.onListItemPress(item)}>
                                        <Avatar
                                            rounded
                                            size="small"
                                            placeholderStyle={{ backgroundColor: vars.COLOR_GRAY_L3 }}
                                            source={{
                                                uri: UserService.getAvatarUrl(item.user)
                                            }}

                                        />
                                        <ListItem.Content>
                                            <ListItem.Title>{`${item.user.givenName} ${item.user.surname}`}</ListItem.Title>
                                        </ListItem.Content>
                                        <ListItem.Chevron
                                            name="trash"
                                            type="font-awesome"
                                            size={vars.ICO_NORMAL}
                                            color={vars.COLOR_ERROR}
                                            onPress={() => this.onRemovePermittedUserPress(item)}
                                        />
                                    </ListItem>
                                )}
                                refreshControl={(
                                    <RefreshControl
                                        refreshing={this.state.isRefreshing}
                                        onRefresh={this.onRefresh}
                                    />
                                )}
                            />
                            <FloatingButton
                                icon="user-plus"
                                onPress={() => {
                                    navigation.navigate(RouteNames.ALLOW_USER_SCREEN, {
                                        tracker: tracker,
                                        onGoBackFunc: () => this.loadItems()
                                    });
                                }}
                            />
                        </View>
                    )
                }
            </View>
        );
    }

}

const styles = StyleSheet.create({
    container: {
        flex: 1
    },
    loadingContainer: {
        flex: 1,
        alignItems: 'center',
        justifyContent: 'center'
    }
});