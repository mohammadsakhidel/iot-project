import React from 'react';
import { View, StyleSheet } from 'react-native';
import { BottomSheet as BottomSheetElement, Button } from 'react-native-elements';
import * as vars from '../../styles/vars';
import Icon from '../Icon';

export default function BottomSheet(props) {

    const {
        onClosePress,
        children,
        ...rest
    } = props;

    return (
        <BottomSheetElement {...rest}>
            <View style={styles.container}>
                <Button
                    icon={<Icon name="times" color={vars.COLOR_GRAY_L2} />}
                    type="clear"
                    style={styles.close}
                    onPress={onClosePress}
                />
                <View>
                    {children}
                </View>
            </View>
        </BottomSheetElement>
    );
}

const styles = StyleSheet.create({
    container: {
        borderTopLeftRadius: 20,
        borderTopRightRadius: 20,
        backgroundColor: vars.COLOR_GRAY_LIGHTEST,
        padding: vars.PAD_NORMAL
    },
    content: {
        padding: vars.PAD_DOUBLE
    },
    close: {
    }
});