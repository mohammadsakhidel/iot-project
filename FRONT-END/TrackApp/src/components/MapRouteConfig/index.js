import React, { useState } from 'react';
import Modal from '../Modal';
import { ListItem } from 'react-native-elements';
import { Strings } from '../../i18n/strings';
import PropTypes from 'prop-types';
import { StyleSheet, View } from 'react-native';
import * as vars from '../../styles/vars';

function MapRouteConfig(props) {

    // State:
    const [selectItems, setSelectItems] = useState([
        { name: Strings.LastHour, value: 'h', selected: true },
        { name: Strings.Last24Hours, value: 'd', selected: false },
        { name: Strings.Last7Days, value: 'w', selected: false },
        { name: Strings.Last30Days, value: 'm', selected: false }
    ]);

    // Props:

    const {
        visible,
        onConfirmPress,
        onBackdropPress
    } = props;

    // Event Handlers:

    const onItemSelectionChange = (index) => {
        if (!selectItems[index].selected) {

            const newSelectItems = [...selectItems];

            newSelectItems.forEach(si => {
                si.selected = false;
            });
            newSelectItems[index].selected = true;

            setSelectItems(newSelectItems);

        }
    };

    // Render:

    return (
        <Modal
            visible={visible}
            title={Strings.RouteConfig}
            onConfirmPress={() => {
                const selectedItem = selectItems.find(i => i.selected);
                if (selectedItem)
                    onConfirmPress(selectedItem);
            }}
            onBackdropPress={onBackdropPress}
        >

            <View style={styles.itemsContainer}>
                {selectItems.map((item, index) => (
                    <ListItem key={index} topDivider={index > 0}
                        onPress={() => onItemSelectionChange(index)}>
                        <ListItem.Content>
                            <ListItem.Title>
                                {item.name}
                            </ListItem.Title>
                        </ListItem.Content>
                        <ListItem.CheckBox
                            checked={selectItems[index].selected}
                            onPress={() => onItemSelectionChange(index)}
                        />
                    </ListItem>
                ))}
            </View>

        </Modal>
    )
}

const styles = StyleSheet.create({
    itemsContainer: {
        paddingTop: vars.PAD_NORMAL
    }
});

MapRouteConfig.propTypes = {
    visible: PropTypes.bool,
    onConfirmPress: PropTypes.func,
    onBackdropPress: PropTypes.func
};

export default MapRouteConfig;
