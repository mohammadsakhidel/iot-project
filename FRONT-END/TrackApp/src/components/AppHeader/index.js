import React from 'react';
import { Strings } from '../../i18n/strings';
import * as globalStyles from '../../styles/global-styles';
import { Header, Icon, Left, Body, Right, Title, Button } from 'native-base';
import { Text, View } from 'react-native';
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
                        <Left>
                            <Button transparent onPress={onLeftPress}>
                                <Icon name={leftIconName} type={leftIconType} />
                            </Button>
                        </Left>
                    )
                    : null}
                <Body>
                    <Title>
                        <Text style={globalStyles.headerTitle}>{Strings.AppHeaderTitle}</Text>
                        <Text style={globalStyles.headerSubtitle}>{Strings.AppHeaderSubtitle}</Text>
                    </Title>
                </Body>
                {hasRight
                    ? (
                        <Right>
                            <Button transparent onPress={onRightPress}>
                                <Icon name={rightIconName} type={rightIconType} />
                            </Button>
                        </Right>
                    )
                    : null
                }
            </Header>
            <StatusBar style="light" backgroundColor={vars.COLOR_SECONDARY} />
        </View>
    );
}