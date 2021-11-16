import React from 'react'
import { StyleSheet } from 'react-native';
import { TouchableHighlight } from 'react-native-gesture-handler';
import PropTypes from 'prop-types';
import * as vars from '../../styles/vars';

function MapButton(props) {

    const {
        first,
        last,
        icon,
        onPress
    } = props;

    return (
        <TouchableHighlight
            style={[
                styles.container,
                (first ? styles.first : null),
                (last ? styles.last : null)
            ]}
            underlayColor={vars.COLOR_GRAY_L3}
            onPress={onPress}>

            {icon}

        </TouchableHighlight>
    )
}

const styles = StyleSheet.create({
    container: {
        backgroundColor: vars.COLOR_GRAY_LIGHTEST,
        borderBottomColor: vars.COLOR_GRAY_L3,
        borderBottomWidth: 1,
        padding: 10
    },
    first: {
        borderTopStartRadius: 10,
        borderTopEndRadius: 10
    },
    last: {
        borderBottomStartRadius: 10,
        borderBottomEndRadius: 10,
        borderBottomWidth: 0
    }
});

MapButton.propTypes = {
    icon: PropTypes.element.isRequired,
    first: PropTypes.bool,
    last: PropTypes.bool,
    onPress: PropTypes.func
};

export default MapButton;
