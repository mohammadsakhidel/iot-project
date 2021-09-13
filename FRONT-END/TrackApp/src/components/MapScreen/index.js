import React, { Component } from 'react';
import { View, Dimensions, StyleSheet, Text } from 'react-native';
import MapView, { Marker } from 'react-native-maps';
import { connect } from 'react-redux';

class MapScreen extends Component {

    constructor(props) {
        super(props);

        // State:
        this.state = {
            message: ''
        };

        // Bindings:
        this.onPress = this.onPress.bind(this);

    }

    componentDidMount() {
    }

    onPress() {
    }

    render() {

        const { locationUpdates } = this.props;

        const initialRegion = {
            latitude: -34.929697,
            longitude: 138.600321,
            latitudeDelta: 0.4,
            longitudeDelta: 0.4
        };

        return (
            <View style={styles.container}>
                <MapView
                    style={styles.map}
                >
                    <Marker coordinate={initialRegion} />
                </MapView>
                <View style={{ padding: 20, backgroundColor: '#f5f5f5', height: 300 }}>
                    <Text>
                        {locationUpdates ? JSON.stringify(locationUpdates): "NULL"}
                    </Text>
                </View>
            </View>
        );
    }
};

const styles = StyleSheet.create({
    container: {
        flex: 1
    },
    map: {
        flex: 1,
        width: Dimensions.get('window').width,
    }
});

const mapStateToProps = (state) => ({
    locationUpdates: state.locationUpdates
});

export default connect(mapStateToProps)(MapScreen);