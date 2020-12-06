import { StatusBar } from 'expo-status-bar';
import React from 'react';
import { StyleSheet, Text, View } from 'react-native';
import { Strings } from '../../i18n/strings';
import Dummy from '../Dummy';

export default function Main() {

  Strings.init();

  return (
    <View style={styles.container}>
      <Dummy>
        {Strings('welcome')}
      </Dummy>
    </View>
  );
}

const styles = StyleSheet.create({
  container: {
    flex: 1,
    backgroundColor: '#fff',
    alignItems: 'center',
    justifyContent: 'center',
  },
});
