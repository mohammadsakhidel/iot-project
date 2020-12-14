import React from 'react';
import { Strings } from '../../i18n/strings';
import * as globalStyles from '../../styles/global-styles';
import { Header, Icon, Left, Body, Right, Title, Button } from 'native-base';
import { Image, Text, View, Platform, StyleSheet } from 'react-native';
import * as vars from '../../styles/vars';
import { StatusBar } from 'expo-status-bar';

export default function AppHeader(props) {

    const {
        hasLeft,
        hasRight,
        onLeftPress,
        onRightPress,
        leftIconName,
        rightIconName,
        leftIconType,
        rightIconType
    } = props;

    return (
        <View>
            <Header style={globalStyles.header}>
                {hasLeft
                    ? (
                        <Button transparent onPress={onLeftPress}>
                            <Icon
                                name={leftIconName}
                                type={leftIconType}
                                style={styles.icons}
                            />
                        </Button>
                    )
                    : null}
                <Body>
                    <View style={{ flext: 1, justifyContent: 'center' }}>
                        <Image
                            source={require('../../styles/images/header-title.png')}
                            style={styles.titleImage}
                            resizeMode="contain"
                        />
                    </View>
                </Body>
                {hasRight
                    ? (
                        <Button transparent onPress={onRightPress}>
                            <Icon
                                name={rightIconName}
                                type={rightIconType}
                                style={styles.icons}
                            />
                        </Button>
                    )
                    : null
                }
            </Header>
            {(Platform.OS == "android"
                ? (<StatusBar style="light" backgroundColor={vars.COLOR_SECONDARY} />)
                : null
            )}
        </View>
    );
}

const styles = StyleSheet.create({
    icons: {
        color: vars.COLOR_GRAY_LIGHTEST,
        fontSize: vars.ICO_NORMAL
    },
    titleImage: {
        width: 100
    }
});