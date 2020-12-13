import React, { useContext } from 'react';
import { Text, View, Button } from 'react-native';
import { connect } from 'react-redux';
import { incCounter } from '../../redux/actions';
import { Strings } from '../../i18n/strings';
import { Icon } from 'native-base';
import AppContext from '../../helpers/app-context';


const TrackersScreen = (props) => {

    const appContext = useContext(AppContext);

    return (
        <View style={{ padding: 30 }}>
            <Text>{Strings.Welcome}</Text>
            <Text>{appContext.user.id}</Text>
            <Text>{appContext.user.givenName}</Text>
            <Text>{appContext.user.surname}</Text>
            <Text>{appContext.user.token}</Text>
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