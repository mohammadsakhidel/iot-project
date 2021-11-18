import React, { useEffect, useState } from 'react';
import PropTypes from 'prop-types';
import { StyleSheet, View } from 'react-native';
import PrimaryButton from '../PrimaryButton';
import { Strings } from '../../i18n/strings';
import * as vars from '../../styles/vars';
import * as GlobalStyles from '../../styles/global-styles';

function MapSavingPanel(props) {

    // State:

    const [saving, setSaving] = useState(false);

    // Props:

    const {
        saveTitle,
        cancelTitle,
        onCancelFunc,
        onSaveFunc,
    } = props;

    // Effects:

    useEffect(() => {

        if (!saving)
            return;

        if (onSaveFunc) {
            (async function () {

                await onSaveFunc();
                setSaving(false);

            })();
        }

    }, [saving]);

    // Event Handlers:

    const onSaveButtonPress = () => {
        setSaving(true);
    };

    // Render:

    return (
        <View style={styles.container}>
            <View style={styles.cancelContainer}>
                <PrimaryButton
                    title={cancelTitle ?? Strings.Cancel}
                    icon="times"
                    style={GlobalStyles.secondaryButton}
                    disabled={saving}
                    onPress={onCancelFunc}
                    disabledStyle={GlobalStyles.secondaryButtonDisabled}
                />
            </View>
            <View style={styles.saveContainer}>
                <PrimaryButton
                    icon="save"
                    title={saveTitle ?? Strings.Save}
                    isLoading={saving}
                    disabled={saving}
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
    onCancelFunc: PropTypes.func
};

export default MapSavingPanel;
