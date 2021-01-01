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
import { formatDateString } from '../../utils/text-util';

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
        const { item } = this.props;

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
                        {this.renderStatus()}
                        <View style={styles.actionsContainer}>
                            <LinkButton
                                icon="cogs"
                                iconStyle={styles.actionIcon}
                                title={Strings.Configure}
                                titleStyle={styles.action}
                                onPress={this.onConfigurePress}
                            />

                            <LinkButton
                                icon="trash"
                                iconStyle={{ ...styles.actionIcon, ...styles.removeActionIcon }}
                                title={Strings.Remove}
                                titleStyle={[styles.action, styles.removeAction]}
                                onPress={this.onRemovePress}
                            />
                        </View>
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

    renderStatus() {

        const { connection } = this.props;

        switch (connection?.status) {
            case null:
            case undefined:
                return (
                    <Text style={GlobalStyles.smallText}>
                        {Strings.StatusLoading}
                    </Text>
                );
            case 'online':
                return (
                    <View style={{ flexDirection: 'row', alignItems: 'center' }}>
                        <Icon name="circle" style={styles.onlineIcon} />
                        <Text style={styles.online}>
                            {Strings.Online}
                        </Text>
                    </View>
                );
            case 'offline':
                return (
                    <View style={{ flexDirection: 'row', alignItems: 'center' }}>
                        <Icon name="circle" style={styles.offlineIcon} />
                        <Text style={styles.offline}>
                            {
                                Strings.Offline +
                                (connection.lastConnection != ''
                                    ? ' ' + Strings.Since + ' ' + formatDateString(connection.lastConnection)
                                    : ''
                                )
                            }
                        </Text>
                    </View>
                );
            case 'error':
                return (
                    <View style={{ flexDirection: 'row', alignItems: 'center' }}>
                        <Text style={styles.errorStatus}>
                            {Strings.ErrorCheckingStatus}
                        </Text>
                    </View>
                );
            default:
                return null;
        }
    }
}

/* #region  Styles */
const styles = StyleSheet.create({
    container: {
        padding: vars.PAD_NORMAL,
        borderBottomWidth: StyleSheet.hairlineWidth,
        borderBottomColor: vars.COLOR_GRAY_L3,
        flexDirection: 'row'
    },
    textContainer: {
        marginHorizontal: vars.PAD_HALF
        //marginTop: vars.PAD_HALF
    },
    icon: {
        width: 70,
        height: 70,
        borderRadius: 10
    },
    actionsContainer: {
        flexDirection: 'row'
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
    online: {
        ...GlobalStyles.smallText,
        ...GlobalStyles.success,
        marginHorizontal: vars.PAD_HALF
    },
    onlineIcon: {
        ...GlobalStyles.success,
        fontSize: vars.ICO_TINY
    },
    offline: {
        ...GlobalStyles.smallText,
        marginHorizontal: vars.PAD_HALF
    },
    offlineIcon: {
        color: vars.COLOR_GRAY_L1,
        fontSize: vars.ICO_TINY
    },
    errorStatus: {
        ...GlobalStyles.smallText,
        ...GlobalStyles.error,
        marginHorizontal: vars.PAD_HALF
    }
});
/* #endregion */