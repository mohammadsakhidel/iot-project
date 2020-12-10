import React, { useContext } from 'react';
import { Text, View, Button } from 'react-native';
import { connect } from 'react-redux';
import { incCounter } from '../../redux/actions';
import { Strings } from '../../i18n/strings';
import { Icon } from 'native-base';


const TrackersScreen = (props) => {
    return (
        <View style={{ padding: 30 }}>
            <Text>{Strings.Welcome}</Text>
            <Text style={{fontSize: 14}}>{Strings.Welcome}</Text>
            <Icon name="apps" />

        </View>
    );
};

const mapStateToProps = state => ({
    counter: state.counter
});

const mapDispatchToProps = dispatch => ({
    incCounter: () => dispatch(incCounter())
});

export default connect(mapStateToProps, mapDispatchToProps)(TrackersScreen);