import React from 'react';
import { Text } from 'react-native';
import { connect } from 'react-redux';
import { incCounter } from '../../redux/actions';

const HomeScreen = (props) => {
    return (
        <Text>Counter: {props.counter}</Text>
    );
};

const mapStateToProps = state => ({
    counter: state.counter
});

const mapDispatchToProps = dispatch => ({
    incCounter: () => dispatch(incCounter())
});

export default connect(mapStateToProps, mapDispatchToProps)(HomeScreen);