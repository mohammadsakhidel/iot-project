import React, { Component } from 'react';
import { View, StyleSheet, ActivityIndicator, FlatList } from 'react-native';
import AppContext from '../../helpers/app-context';
import TrackerService from '../../api/services/tracker-service';
import TrackerItem from '../TrackerItem';
import { showError } from '../FlashMessageWrapper';
import Loading from '../Loading';
import RefreshControl from '../RefreshControl';

export default class TrackersScreen extends Component {

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
    }

    async componentDidMount() {
        await this.loadTrackers();
    }

    async loadTrackers() {
        try {

            const result = await TrackerService.list(this.context.user.token);
            if (result.done) {
                setTimeout(() => {
                    this.setState({
                        isLoading: false,
                        isRefreshing: false,
                        trackers: result.data
                    });
                }, 2000);
            } else {
                this.setState({ isLoading: false, isRefreshing: false });
                throw new Error(result.data);
            }

        } catch (e) {
            showError(e);
        }
    };

    onRefresh() {
        this.setState({
            isLoading: true,
            isRefreshing: true
        }, async () => {
            await this.loadTrackers();
        });
    };

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
                                    <TrackerItem item={item} />
                                )}
                            />
                        )
                }
            </View>
        );
    }
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