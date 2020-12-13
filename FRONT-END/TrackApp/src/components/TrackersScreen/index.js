import React, { useContext, useEffect, useState } from 'react';
import { Text, View, Button, StyleSheet, ScrollView, ActivityIndicator, FlatList } from 'react-native';
import { connect } from 'react-redux';
import { incCounter } from '../../redux/actions';
import { Strings } from '../../i18n/strings';
import { Icon } from 'native-base';
import AppContext from '../../helpers/app-context';
import TrackerService from '../../api/services/tracker-service';

export default function TrackersScreen(props) {

    const appContext = useContext(AppContext);
    const [state, setState] = useState({
        trackers: [],
        isLoading: true,
        error: ''
    });

    // Load Trackers: 
    useEffect(() => {
        TrackerService.list(appContext.user.token).then(result => {
            if (result.done) {
                setTimeout(() => {
                    setState({ ...state, isLoading: false, trackers: result.data });
                }, 2000);
            } else {
                setState({ ...state, isLoading: false, error: Strings.ErrorMessageLoading });
            }
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
                            <Text style={{ paddingVertical: 200, borderBottomColor: 'red', borderBottomWidth: 2 }}>
                                {item.id}
                            </Text>
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