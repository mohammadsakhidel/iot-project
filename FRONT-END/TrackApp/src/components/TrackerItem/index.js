import React, { Component } from 'react';
import { View, StyleSheet, Alert, TouchableHighlight } from 'react-native';
import Text from '../Text';
import * as vars from '../../styles/vars';
import { Image } from 'react-native-elements';
import TrackerService from '../../api/services/tracker-service';
import { Strings } from '../../i18n/strings';
import LinkButton from '../LinkButton';
import Loading from '../Loading';
import { showError } from '../FlashMessageWrapper';
import * as GlobalStyles from '../../styles/global-styles';
import AppContext from '../../helpers/app-context';
import Icon from '../Icon';

import TrackerStatus from '../TrackerStatus';

export default class TrackerItem extends Component {

    static contextType = AppContext;

    constructor(props) {
        super(props);

        // State:
        this.state = {
            isLoading: false,
            status: null,
            lastConnect: null
        };

        // Bindings:
        this.onRemovePress = this.onRemovePress.bind(this);
        this.onConfigurePress = this.onConfigurePress.bind(this);
    }

    render() {
        const { item, connection } = this.props;

        return (
            <TouchableHighlight onPress={this.onConfigurePress} underlayColor={vars.COLOR_GRAY_L3}>
                <View style={styles.container}>
                    <Image
                        source={{ uri: TrackerService.getIconUrl(item) }}
                        style={styles.icon}
                        PlaceholderContent={<Loading size="small" color={vars.COLOR_GRAY_L2} />}
                        placeholderStyle={{ backgroundColor: vars.COLOR_GRAY_LIGHTEST }}
                    />
                    <View style={styles.textContainer}>
                        <Text bold>{item.displayName}</Text>
                        <TrackerStatus status={connection.status} lastConnection={connection.lastConnection} />
                    </View>
                    <View style={styles.actionsContainer}>
                        {/* <LinkButton
                            icon="cogs"
                            iconStyle={styles.actionIcon}
                            title={Strings.Configure}
                            titleStyle={styles.action}
                            onPress={this.onConfigurePress}
                        /> */}

                        <LinkButton
                            icon="trash"
                            iconStyle={{ ...styles.actionIcon, ...styles.removeActionIcon }}
                            title=""
                            titleStyle={styles.removeAction}
                            onPress={this.onRemovePress}
                        />
                    </View>
                    {this.state.isLoading ? (
                        <View style={styles.loading}>
                            <Loading size="small" />
                        </View>
                    ) : null}
                </View>
            </TouchableHighlight>
        );
    }

    onRemovePress() {

        const {
            item,
            removeTrackerFunc,
            reloadDataFunc
        } = this.props;

        Alert.alert(
            item.displayName,
            Strings.RemoveTrackerSureMessage,
            [
                {
                    text: Strings.Yes,
                    onPress: () => {
                        this.setState({ isLoading: true }, async () => {
                            try {
                                await removeTrackerFunc(item);
                                await reloadDataFunc();
                            } catch (e) {
                                this.setState({ isLoading: false });
                                showError(e);
                            }
                        });
                    }
                },
                {
                    text: Strings.Cancel,
                    onPress: () => { }
                }
            ]
        );

    };

    onConfigurePress() {
        const {
            item,
            configureTrackerFunc
        } = this.props;

        configureTrackerFunc(item);
    }

}

/* #region  Styles */
const styles = StyleSheet.create({
    container: {
        paddingVertical: vars.PAD_NORMAL,
        borderBottomWidth: StyleSheet.hairlineWidth,
        borderBottomColor: vars.COLOR_GRAY_L3,
        flexDirection: 'row',
        alignItems: 'stretch'
    },
    textContainer: {
        flex: 1,
        justifyContent: 'center'
    },
    icon: {
        width: 65,
        height: 65,
        borderRadius: 10,
        marginHorizontal: vars.PAD_NORMAL
    },
    actionsContainer: {
        justifyContent: 'center',
        paddingRight: vars.PAD_NORMAL
    },
    action: {
        fontSize: vars.FS_BIT_SMALLER
    },
    removeAction: {
        color: vars.COLOR_ERROR
    },
    actionIcon: {
        fontSize: vars.ICO_SMALL
    },
    removeActionIcon: {
        color: vars.COLOR_ERROR
    },
    loading: {
        position: 'absolute',
        left: 0,
        top: 0,
        right: 0,
        bottom: 0,
        opacity: 0.85,
        backgroundColor: vars.COLOR_GRAY_LIGHTEST,
        alignItems: 'center',
        justifyContent: 'center'
    },

});
/* #endregion */