import React, { useEffect, useState } from 'react';
import { View, StyleSheet } from 'react-native';
import Text from '../Text';
import { Overlay } from 'react-native-elements';
import * as vars from '../../styles/vars';
import * as globalStyles from '../../styles/global-styles';
import PrimaryButton from '../PrimaryButton';
import { Strings } from '../../i18n/strings';
import PropTypes from 'prop-types';

export default function Modal(props) {

    // Props:
    const {
        children,
        title,
        visible,
        onConfirmPress,
        onBackdropPress,
        loading,
        ...rest
    } = props;

    return (
        <Overlay visible={visible}
            overlayStyle={styles.overlay}
            onBackdropPress={onBackdropPress}
            {...rest}
        >

            <View>
                <Text style={globalStyles.modalTitle}>
                    {title}
                </Text>

                {children}

                <View style={styles.buttonsContainer}>
                    <PrimaryButton
                        title={Strings.Confirm}
                        onPress={onConfirmPress}
                        isLoading={loading}
                        disabled={loading}
                    />
                </View>
            </View>

        </Overlay>
    );

}

const styles = StyleSheet.create({
    repeatTitle: {
        fontWeight: 'bold',
        fontSize: vars.FS_BIT_LARGER
    },
    overlay: {
        margin: vars.PAD_TRIPPLE,
        alignSelf: 'stretch'
    },
    buttonsContainer: {
        marginTop: vars.PAD_NORMAL
    }
});

Modal.propTypes = {
    title: PropTypes.string,
    visible: PropTypes.bool,
    onConfirmPress: PropTypes.func,
    onBackdropPress: PropTypes.func,
    loading: PropTypes.bool
};