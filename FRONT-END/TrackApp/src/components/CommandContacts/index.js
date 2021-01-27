import React, { Component } from 'react';
import { StyleSheet, Alert, View, LogBox } from 'react-native';
import * as vars from '../../styles/vars';
import CommandService from '../../api/services/command-service';
import AppContext from '../../helpers/app-context';
import { Strings } from '../../i18n/strings';
import * as RouteNames from '../../constants/route-names';
import { showError } from '../FlashMessageWrapper';
import { ListItem, Avatar } from 'react-native-elements';
import List from '../List';
import Loading from '../Loading';
import UserService from '../../api/services/user-service';
import RefreshControl from '../RefreshControl';
import FloatingButton from '../FloatingButton';
import LoadingOver from '../LoadingOver';

export default class CommandContacts extends Component {

    static contextType = AppContext;

    constructor(props) {
        super(props);

        // State:
        this.state = {
            isLoading: false,
            isRefreshing: false,
            items: null
        };

        // Bindings:
        this.loadItems = this.loadItems.bind(this);
        this.onRefresh = this.onRefresh.bind(this);
        this.onRemoveItemPress = this.onRemoveItemPress.bind(this);
        this.removeItem = this.removeItem.bind(this);

    }

    componentDidMount() {
        this.loadItems();
    }

    loadItems() {
        this.setState({ isLoading: true }, async () => {
            try {

                const { tracker } = this.props;
                const result = await CommandService.getContacts(tracker.id, this.context.user.token);

                if (!result.done)
                    throw new Error(result.data);

                const contacts = result.data;

                this.setState({
                    isLoading: false,
                    isRefreshing: false,
                    items: contacts ?? []
                });

            } catch (e) {
                this.setState({ isLoading: false, isRefreshing: false });
                showError(e);
            }
        });
    }

    onRefresh() {
        this.setState({ isRefreshing: true }, () => {
            this.loadItems();
        });
    }

    onRemoveItemPress(item) {
        try {

            Alert.alert(
                Strings.AreYouSure,
                Strings.AreYouSureRemoveItem,
                [
                    {
                        text: Strings.Yes,
                        onPress: () => this.removeItem(item)
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

    removeItem(item) {
        this.setState({ isLoading: true }, async () => {
            try {

                const { tracker } = this.props;
                const { number } = item;

                const result = await CommandService.removeContact(tracker.id, number, this.context.user.token);
                if (result.done) {
                    this.loadItems();
                }

            } catch (e) {
                this.setState({ isLoading: false });
                showError(e);
            }
        });
    }

    render() {

        const { tracker } = this.props;
        const { navigation } = this.props;

        return (
            <View style={styles.container}>
                <View style={{ flex: 1 }}>
                    {this.state.items != null ? (
                        <List
                            emptyListMessage={Strings.EmptyListMessage}
                            reloadFunc={this.loadItems}
                            data={this.state.items}
                            keyExtractor={(item) => item.number}
                            renderItem={({ item }) => (
                                <ListItem key={item.number} bottomDivider>
                                    <Avatar
                                        rounded
                                        size="small"
                                        placeholderStyle={{ backgroundColor: vars.COLOR_GRAY_L3 }}
                                        source={{
                                            uri: UserService.getDefaultUserAvatarUrl()
                                        }}

                                    />
                                    <ListItem.Content>
                                        <ListItem.Title>{`${item.name}`}</ListItem.Title>
                                        <ListItem.Subtitle>{`${item.number}`}</ListItem.Subtitle>
                                    </ListItem.Content>
                                    <ListItem.Chevron
                                        name="trash"
                                        type="font-awesome"
                                        size={vars.ICO_NORMAL}
                                        color={vars.COLOR_ERROR}
                                        onPress={() => this.onRemoveItemPress(item)}
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
                    ) : null}
                    <LoadingOver loading={this.state.isLoading} />
                    <FloatingButton
                        icon="user-plus"
                        onPress={() => {
                            navigation.navigate(RouteNames.ADD_CONTACT, {
                                tracker: tracker,
                                onGoBackFunc: () => this.loadItems()
                            });
                        }}
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
    loadingContainer: {
        flex: 1,
        alignItems: 'center',
        justifyContent: 'center'
    },
    nameContainer: {
        flexDirection: 'row',
        alignItems: 'center'
    },
    name: {
        flex: 1,
        fontSize: vars.FS_BIT_LARGER,
        color: vars.COLOR_GRAY_L1
    },
    remove: {

    }
});