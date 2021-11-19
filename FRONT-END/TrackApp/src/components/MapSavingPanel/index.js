import React, { useEffect, useRef, useState } from 'react';
import PropTypes from 'prop-types';
import { StyleSheet, View } from 'react-native';
import PrimaryButton from '../PrimaryButton';
import { Strings } from '../../i18n/strings';
import * as vars from '../../styles/vars';
import * as GlobalStyles from '../../styles/global-styles';

function MapSavingPanel(props) {

    // State:

    const [saving, setSaving] = useState(false);
    const [cancelling, setCancelling] = useState(false);

    // Props:

    const {
        saveTitle,
        cancelTitle,
        onCancelFunc,
        onSaveFunc,
        cancelIcon
    } = props;

    // Refs:

    const mountedRef = useRef(true);

    // Effects:

    useEffect(() => {
        return () => {
            mountedRef.current = false;
        };
    }, []);

    useEffect(() => {

        if (!saving)
            return;

        if (onSaveFunc) {
            (async function () {

                await onSaveFunc();

                if (mountedRef.current)
                    setSaving(false);

            })();
        }

    }, [saving]);

    useEffect(() => {

        if (!cancelling)
            return;

        if (onCancelFunc) {
            (async function () {

                await onCancelFunc();

                if (mountedRef.current)
                    setCancelling(false);

            })();
        }

    }, [cancelling]);

    // Event Handlers:

    const onSaveButtonPress = () => {
        setSaving(true);
    };

    const onCancelButtonPress = () => {
        setCancelling(true);
    };

    // Render:

    return (
        <View style={styles.container}>
            <View style={styles.cancelContainer}>
                <PrimaryButton
                    title={cancelTitle ?? Strings.Cancel}
                    icon={(cancelIcon ?? "times")}
                    style={GlobalStyles.secondaryButton}
                    isLoading={cancelling}
                    disabled={saving || cancelling}
                    onPress={onCancelButtonPress}
                    disabledStyle={GlobalStyles.secondaryButtonDisabled}
                />
            </View>
            <View style={styles.saveContainer}>
                <PrimaryButton
                    icon="save"
                    title={saveTitle ?? Strings.Save}
                    isLoading={saving}
                    disabled={saving || cancelling}
                    onPress={onSaveButtonPress}
                />
            </View>
        </View>
    );
}

const styles = StyleSheet.create({
    container: {
        flexDirection: 'row',
        padding: vars.PAD_NORMAL
    },
    cancelContainer: {
        flex: 1,
        marginEnd: vars.PAD_HALF
    },
    saveContainer: {
        flex: 1,
        marginStart: vars.PAD_HALF
    },
    notReadyContainer: {

    }
});

MapSavingPanel.propTypes = {
    saving: PropTypes.bool,
    saveTitle: PropTypes.string,
    onSaveFunc: PropTypes.func,
    cancelTitle: PropTypes.string,
    onCancelFunc: PropTypes.func,
    cancelIcon: PropTypes.string
};

export default MapSavingPanel;
