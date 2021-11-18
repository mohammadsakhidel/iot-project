import React from 'react'
import { StyleSheet, View, ViewPropTypes } from 'react-native';
import MapButton from '../MapButton';
import Icon from '../Icon';
import * as vars from '../../styles/vars';
import PropTypes from 'prop-types';

function MapToolBox(props) {

    const {
        onRoutePress,
        onPolygonPress,
        onFitAllPress,
        routeSelected,
        polygonSelected
    } = props;

    return (
        <View style={styles.container}>
            <MapButton
                first
                icon={
                    <Icon name="route" color={vars.COLOR_SECONDARY_L1} />
                }
                onPress={onRoutePress}
                selected={routeSelected}
            />

            <MapButton
                icon={
                    <Icon name="draw-polygon" color={vars.COLOR_SECONDARY_L1} />
                }
                onPress={onPolygonPress}
                selected={polygonSelected}
            />

            <MapButton
                last
                icon={
                    <Icon name="compress-arrows-alt" color={vars.COLOR_SECONDARY_L1} />
                }
                onPress={onFitAllPress}
            />
        </View>
    )
}

const styles = StyleSheet.create({
    container: {
        position: 'absolute',
        top: vars.PAD_TRIPPLE,
        left: vars.PAD_DOUBLE
    }
});

MapToolBox.propTypes = {
    onRoutePress: PropTypes.func,
    onPolygonPress: PropTypes.func,
    onFitAllPress: PropTypes.func,
    routeSelected: PropTypes.bool,
    polygonSelected: PropTypes.bool
};

export default MapToolBox;
