import React, { useContext, useEffect, useState } from 'react';
import { Text, View, Button, StyleSheet, ScrollView, ActivityIndicator, FlatList, Alert } from 'react-native';
import { connect } from 'react-redux';
import { incCounter } from '../../redux/actions';
import { Strings } from '../../i18n/strings';
import { Icon } from 'native-base';
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
        console.log(appContext.user.token);
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