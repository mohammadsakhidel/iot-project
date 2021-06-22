import React from 'react';
import { View, StyleSheet } from 'react-native';
import { BottomSheet as BottomSheetElement } from 'react-native-elements';
import * as vars from '../../styles/vars';
import Icon from '../Icon';
import Button from '../Button';

export default function BottomSheet(props) {

    const {
        onClosePress,
        children,
        ...rest
    } = props;

    return (
        <BottomSheetElement {...rest}>
            <View style={styles.container}>
                <View>
                    {children}
                </View>

                <Button
                    icon={
                        <Icon name="times"
                            color={vars.COLOR_GRAY_L2}
                            size={vars.ICO_BIT_SMALLER} />
                    }
                    type="clear"
                    containerStyle={styles.closeButton}
                    onPress={onClosePress}
                />
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
    closeButton: {
        position: 'absolute',
        end: vars.PAD_NORMAL,
        top: vars.PAD_NORMAL
    }
});