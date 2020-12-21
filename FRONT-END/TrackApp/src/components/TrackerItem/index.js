import React, { Component } from 'react';
import { View, StyleSheet, Alert } from 'react-native';
import Text from '../Text';
import * as vars from '../../styles/vars';
import { Image } from 'react-native-elements';
import TrackerService from '../../api/services/tracker-service';
import { Strings } from '../../i18n/strings';
import LinkButton from '../LinkButton';
import Loading from '../Loading';
import { showError } from '../FlashMessageWrapper';

export default class TrackerItem extends Component {

    constructor(props) {
        super(props);

        // State:
        this.state = {
            isLoading: false
        };

        // Bindings:
        this.onRemovePress = this.onRemovePress.bind(this);
        this.onConfigurePress = this.onConfigurePress.bind(this);
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

    render() {
        const { item } = this.props;

        return (
            <View style={styles.container}>
                <Image
                    source={{ uri: TrackerService.getIconUrl(item) }}
                    style={styles.icon}
                    PlaceholderContent={<Loading size="small" />}
                />
                <View style={styles.textContainer}>
                    <Text bold>{item.displayName}</Text>
                    <View style={styles.actionsContainer}>
                        <LinkButton
                            icon="cog"
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
                { this.state.isLoading ? (
                    <View style={styles.loading}>
                        <Loading size="small" />
                    </View>
                ) : null}
            </View>
        );
    }
}

/* #region  Styles */
const styles = StyleSheet.create({
    container: {
        padding: vars.PAD_BIT_MORE,
        borderBottomWidth: StyleSheet.hairlineWidth,
        borderBottomColor: vars.COLOR_GRAY_L3,
        flexDirection: 'row'
    },
    textContainer: {
        marginHorizontal: vars.PAD_HALF,
        marginTop: vars.PAD_HALF
    },
    icon: {
        width: 70,
        height: 70
    },
    actionsContainer: {
        flexDirection: 'row',
        marginTop: vars.PAD_HALF
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
    }
});
/* #endregion */