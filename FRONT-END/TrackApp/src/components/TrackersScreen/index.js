import React, { Component } from 'react';
import { View, StyleSheet, ActivityIndicator, FlatList, Alert } from 'react-native';
import AppContext from '../../helpers/app-context';
import TrackerService from '../../api/services/tracker-service';
import TrackerItem from '../TrackerItem';
import { showError } from '../FlashMessageWrapper';
import Loading from '../Loading';
import RefreshControl from '../RefreshControl';
import { Strings } from '../../i18n/strings';

export default class TrackersScreen extends Component {


    /* #region  Component Lifecycle */
    static contextType = AppContext;

    constructor(props) {
        super(props);

        this.state = {
            trackers: [],
            isLoading: true,
            isRefreshing: false,
            error: ''
        };

        // Binding Methods:
        this.loadTrackers = this.loadTrackers.bind(this);
        this.onRefresh = this.onRefresh.bind(this);
        this.removeTrackerFunc = this.removeTrackerFunc.bind(this);
        this.configureTrackerFunc = this.configureTrackerFunc.bind(this);
        this.reloadDataFunc = this.reloadDataFunc.bind(this);
    }

    async componentDidMount() {
        await this.loadTrackers();
    }

    render() {
        return (
            <View style={styles.container} >
                {
                    this.state.isLoading
                        ? (
                            <View style={{ flex: 1, alignItems: 'center', justifyContent: 'center' }}>
                                <Loading size="large" />
                            </View>
                        )
                        : (
                            <FlatList
                                data={this.state.trackers}
                                refreshControl={(
                                    <RefreshControl
                                        refreshing={this.state.isRefreshing}
                                        onRefresh={this.onRefresh}
                                    />
                                )}
                                renderItem={({ item }) => (
                                    <TrackerItem
                                        item={item}
                                        removeTrackerFunc={this.removeTrackerFunc}
                                        configureTrackerFunc={this.configureTrackerFunc}
                                        reloadDataFunc={this.reloadDataFunc}
                                    />
                                )}
                            />
                        )
                }
            </View>
        );
    }
    /* #endregion */

    /* #region  Event Handlers */
    onRefresh() {
        this.setState({
            isLoading: true,
            isRefreshing: true
        }, async () => {
            await this.loadTrackers();
        });
    };
    /* #endregion */

    /* #region  Methods */
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