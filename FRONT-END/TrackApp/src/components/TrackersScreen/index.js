import React, { useContext, useEffect, useState } from 'react';
import { View, StyleSheet, ActivityIndicator, FlatList } from 'react-native';
import AppContext from '../../helpers/app-context';
import TrackerService from '../../api/services/tracker-service';
import TrackerItem from '../TrackerItem';
import { showError } from '../FlashMessageWrapper';

export default function TrackersScreen(props) {

    const appContext = useContext(AppContext);
    const [state, setState] = useState({
        trackers: [],
        isLoading: true,
        error: ''
    });

    // Load Trackers: 
    useEffect(() => {
        TrackerService.list(appContext.user.token)
            .then(result => {
                if (result.done) {
                    setTimeout(() => {
                        setState({ ...state, isLoading: false, trackers: result.data });
                    }, 2000);
                } else {
                    setState({ ...state, isLoading: false });
                    throw new Error(result.data);
                }
            })
            .catch(error => {
                showError(error);
            });
    }, []);

    return (
        <View style={styles.container}>
            {state.isLoading
                ? (
                    <ActivityIndicator size="large" color="red" />
                )
                : (
                    <FlatList
                        data={state.trackers}
                        renderItem={({ item }) => (
                            <TrackerItem item={item} />
                        )}
                    />
                )
            }
        </View>
    );
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