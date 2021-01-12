import React, { Component } from 'react';
import { StyleSheet } from 'react-native';
import { createDrawerNavigator } from '@react-navigation/drawer';
import HomeScreen from '../HomeScreen';
import { Strings } from '../../i18n/strings';
import DrawerContent from '../DrawerContent';
import AppContext from '../../helpers/app-context';
import * as EventNames from '../../constants/event-names';
import { showError } from '../FlashMessageWrapper';
import EventsService from '../../api/services/events-service';
import { connect } from 'react-redux';
import * as Actions from '../../redux/actions';

const Drawer = createDrawerNavigator();

class HomeContainer extends Component {

  static contextType = AppContext;

  constructor(props) {
    super(props);

    // Bindings:
    this.getAccessCodeAndConnect = this.getAccessCodeAndConnect.bind(this);
    this.wsConnect = this.wsConnect.bind(this);
    this.wsOnOpen = this.wsOnOpen.bind(this);
    this.wsOnMessage = this.wsOnMessage.bind(this);
    this.wsOnClose = this.wsOnClose.bind(this);
    this.wsOnError = this.wsOnError.bind(this);

  }

  componentDidMount() {
    setTimeout(async () => {
      await this.getAccessCodeAndConnect();
    }, 500);
  }

  render() {
    return (
      <Drawer.Navigator drawerStyle={styles.drawer}
        drawerContent={() => <DrawerContent user={this.context.user} />}>

        <Drawer.Screen name="HomeScreen" component={HomeScreen} options={{ title: Strings.HomeScreen }} />

      </Drawer.Navigator>
    );
  }

  async getAccessCodeAndConnect() {

    // Get Connections Info from API:
    let connectionsInfo = null;
    while (connectionsInfo == null) {
      try {

        const token = this.context.user.token;
        const apiResult = await EventsService.getConnectionInfoAsync(token);
        if (apiResult.done) {

          connectionsInfo = apiResult.data;

        }

      } catch { }
    }

    // Connect to WebSocket servers:
    try {

      const { accessCode, servers } = connectionsInfo;
      servers.forEach(server => {
        this.wsConnect(server[0], server[1], accessCode);
      });

    } catch (e) {
      showError(e);
    }

  }

  wsConnect(server, port, accessCode) {
    if (!server || !port || !accessCode)
      return;

    this.accessCode = accessCode;
    this.ws = new WebSocket(`ws://${server}:${port}`);

    this.ws.onopen = this.wsOnOpen;
    this.ws.onmessage = this.wsOnMessage;
    this.ws.onclose = this.wsOnClose;
    this.ws.onerror = this.wsOnError;
  }

  wsOnOpen(e) {
    try {
      if (this.ws) {
        this.ws.send(this.accessCode);
      }
    } catch (e) {
      showError(e);
    }
  }

  wsOnMessage(e) {
    try {
      //console.log(e.data);
      const event = JSON.parse(e.data);

      const {
        changeTrackerStatus
      } = this.props;

      switch (event.name) {
        case EventNames.STATUS_CHANGED:
          changeTrackerStatus(event);
          break;
        default:
          break;
      }
    } catch (e) {
      showError(e);
    }
  }

  wsOnClose(e) {
    setTimeout(async () => {
      await this.getAccessCodeAndConnect()
    }, 1000);
  }

  wsOnError(e) {
    //console.log('WS Connection Error.\n' + JSON.stringify(e));
  }

}

const styles = StyleSheet.create({
  container: {
    flex: 1
  },
  drawer: {
  }
});

const mapStateToProps = (state) => ({
  trackers: state.trackers,
  connections: state.connections
});

const mapDispatchToProps = (dispatch) => ({
  changeTrackerStatus: (event) => {
    dispatch(Actions.changeTrackerStatus(event))
  }
});

export default connect(mapStateToProps, mapDispatchToProps)(HomeContainer);