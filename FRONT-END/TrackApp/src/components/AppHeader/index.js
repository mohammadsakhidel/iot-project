import React from 'react';
import * as globalStyles from '../../styles/global-styles';
import { Header } from 'react-native-elements';
import { Image, View, Platform, StyleSheet } from 'react-native';
import * as vars from '../../styles/vars';
import { StatusBar } from 'expo-status-bar';

export default function AppHeader(props) {

    const {
        hasLeft,
        hasRight,
        onLeftPress,
        onRightPress,
        leftIconName,
        rightIconName
    } = props;

    return (
        <View>
            <Header
                style={globalStyles.header}
                leftComponent={hasLeft
                    ? {
                        icon: leftIconName,
                        color: vars.COLOR_GRAY_LIGHTEST,
                        onPress: onLeftPress
                    }
                    : null
                }
                rightComponent={hasRight
                    ? {
                        icon: rightIconName,
                        color: vars.COLOR_GRAY_LIGHTEST,
                        onPress: onRightPress
                    }
                    : null
                }
                centerComponent={(
                    <Image
                        source={require('../../styles/images/header-title.png')}
                        style={styles.titleImage}
                        resizeMode="contain"
                    />
                )}
                containerStyle={globalStyles.header}
            />

            {
                (Platform.OS == "android"
                    ? (<StatusBar style="light" backgroundColor={vars.COLOR_SECONDARY} />)
                    : null
                )
            }
        </View >
    );
}

const styles = StyleSheet.create({
    titleImage: {
        width: 100
    }
});