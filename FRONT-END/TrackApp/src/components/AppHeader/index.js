import React from 'react';
import { Strings } from '../../i18n/strings';
import * as globalStyles from '../../styles/global-styles';
import { Header, Icon, Left, Body, Right, Title, Button } from 'native-base';
import { Text } from 'react-native';

export default function AppHeader({navigation}) {

    // Event Handlers:
    const onPressMenu = () => {
        navigation.openDrawer();
    };

    return (
        <Header style={globalStyles.header}>
            <Left>
                <Button transparent onPress={onPressMenu}>
                    <Icon name="menu" />
                </Button>
            </Left>
            <Body>
                <Title>
                    <Text style={globalStyles.headerTitle}>{Strings.AppHeaderTitle}</Text>
                    <Text style={globalStyles.headerSubtitle}>{Strings.AppHeaderSubtitle}</Text>
                </Title>
            </Body>
            <Right>
                <Button transparent>
                    <Icon name="ellipsis-v" type="FontAwesome" />
                </Button>
            </Right>
        </Header>
    );
}