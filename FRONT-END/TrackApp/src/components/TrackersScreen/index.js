import React, { Component } from 'react';
import { View, StyleSheet, Alert } from 'react-native';
import AppContext from '../../helpers/app-context';
import TrackerService from '../../api/services/tracker-service';
import TrackerItem from '../TrackerItem';
import { showError } from '../FlashMessageWrapper';
import Loading from '../Loading';
import RefreshControl from '../RefreshControl';
import List from '../List';
import { Strings } from '../../i18n/strings';
import FloatingButton from '../FloatingButton';
import { NavigationContext } from '@react-navigation/native';
import * as RouteNames from '../../constants/route-names';
import { getRandom } from '../../utils/text-util';
import Store from '../../redux/store';

export default class TrackersScreen extends Component {
    /* #region  Component Lifecycle */
    static contextType = AppContext;

    constructor(props) {
        super(props);

        // Binding Methods:
        this.loadTrackers = this.loadTrackers.bind(this);
        this.onRefresh = this.onRefresh.bind(this);
        this.removeTrackerFunc = this.removeTrackerFunc.bind(this);
        this.configureTrackerFunc = this.configureTrackerFunc.bind(this);
        this.reloadDataFunc = this.reloadDataFunc.bind(this);
        this.onAddTracker = this.onAddTracker.bind(this);
        this.storeSubscription = this.storeSubscription.bind(this);
        this.retryLoadingFunc = this.retryLoadingFunc.bind(this);

        // State:
        this.state = {
            trackers: [],
            isLoading: true,
            isRefreshing: false,
            error: '',
            dialogKey: '',
            unsubscribeStore: Store.subscribe(this.storeSubscription)
        };
    }

    async componentDidMount() {
        await this.loadTrackers();
    }

    componentWillUnmount() {
        if (this.state.unsubscribeStore)
            this.state.unsubscribeStore();
    }

    render() {
        return (
            <View style={styles.container}>
                {this.state.isLoading
                    ? (
                        <View style={{ flex: 1, alignItems: 'center', justifyContent: 'center' }}>
                            <Loading size="large" />
                        </View>
                    )
                    : (
                        <NavigationContext.Consumer>
                            {navigation => (
                                <View style={{ flex: 1 }}>
                                    <List
                                        emptyListMessage={Strings.EmptyTrackersList}
                                        reloadFunc={this.retryLoadingFunc}
                                        data={this.state.trackers}
                                        renderItem={({ item }) => (
                                            <TrackerItem
                                                item={item}
                                                removeTrackerFunc={this.removeTrackerFunc}
                                                configureTrackerFunc={(tracker) => navigation.navigate(RouteNames.CONFIG_TRACKER, tracker)}
                                                reloadDataFunc={this.reloadDataFunc}
                                            />
                                        )}
                                        refreshControl={(
                                            <RefreshControl
                                                refreshing={this.state.isRefreshing}
                                                onRefresh={this.onRefresh}
                                            />
                                        )}
                                    />
                                    <FloatingButton
                                        icon="plus"
                                        onPress={() => {
                                            this.state.dialogKey = getRandom(6);
                                            navigation.navigate(RouteNames.ADD_TRACKER, { dialogKey: this.state.dialogKey });
                                        }}
                                    />
                                </View>
                            )}
                        </NavigationContext.Consumer>
                    )
                }
            </View>
        );
    }
    /* #endregion */

    /* #region  Event Handlers */
    retryLoadingFunc() {
        this.setState({
            isLoading: true
        }, async () => {
            await this.loadTrackers();
        });
    }

    onRefresh() {
        this.setState({
            isLoading: true,
            isRefreshing: true
        }, async () => {
            await this.loadTrackers();
        });
    };

    onAddTracker() {
        Alert.alert("add tracker");
    }
    /* #endregion */

    /* #region  Methods */
    async storeSubscription() {
        const storeState = Store.getState();
        if (storeState.dialogResult.key == this.state.dialogKey && storeState.dialogResult.value == true) {
            await this.loadTrackers();
        }
    }

    async getTrackersAsync() {
        const result = await TrackerService.list(this.context.user.token);
        if (result.done)
            return result.data;
        else
            throw new Error(result.data);
    }

    async loadTrackers() {
        try {

            const data = await this.getTrackersAsync();
            this.setState({
                isLoading: false,
                isRefreshing: false,
                trackers: data
            });

        } catch (e) {
            this.setState({ isLoading: false, isRefreshing: false });
            showError(e);
        }
    };

    async reloadDataFunc() {
        const data = await this.getTrackersAsync();
        this.setState({ trackers: data });
    }

    async removeTrackerFunc(tracker) {
        await TrackerService.remove(tracker.id, this.context.user.token);
    }

    configureTrackerFunc(tracker) {

    }
    /* #endregion */
};

/* #region  Styles */
const styles = StyleSheet.create({
    container: {
        flex: 1
    },
    loading: {
        alignSelf: 'center',
        margin: 20,
        color: 'red'
    }
});
/* #endregion */
