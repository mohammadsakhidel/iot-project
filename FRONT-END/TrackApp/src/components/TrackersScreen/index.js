import React, { useContext } from 'react';
import { Text, View, Button } from 'react-native';
import { connect } from 'react-redux';
import { incCounter } from '../../redux/actions';
import Strings from '../../i18n/strings';


const TrackersScreen = (props) => {
    return (
        <Text>{Strings.get('welcome')}</Text>
    );
};

const mapStateToProps = state => ({
    counter: state.counter
});

const mapDispatchToProps = dispatch => ({
    incCounter: () => dispatch(incCounter())
});

export default connect(mapStateToProps, mapDispatchToProps)(TrackersScreen);