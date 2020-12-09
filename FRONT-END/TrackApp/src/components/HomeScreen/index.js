import React, { useContext } from 'react';
import { Text, View, Button } from 'react-native';
import { connect } from 'react-redux';
import { incCounter } from '../../redux/actions';
import AppContext from '../../contexts/app-context';

const HomeScreen = (props) => {

    const appContext = useContext(AppContext);

    const changeLanguage = () => {
        let newLang = (appContext.language == 'en-US' ? 'fr-FR' : 'en-US');
        appContext.setLanguage(newLang);
    };


    return (
        <View>
            <Text>Lang: {appContext.language}</Text>
            <Button title='Change Language' onPress={changeLanguage}></Button>
        </View>
    );
};

const mapStateToProps = state => ({
    counter: state.counter
});

const mapDispatchToProps = dispatch => ({
    incCounter: () => dispatch(incCounter())
});

export default connect(mapStateToProps, mapDispatchToProps)(HomeScreen);