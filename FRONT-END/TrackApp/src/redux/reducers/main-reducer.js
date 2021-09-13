import * as Actions from '../actions';

const initialState = {
    trackers: [],
    connections: {},
    locationUpdates: {}
};

const mainReducer = (state = initialState, action) => {

    const event = action.payload;

    switch (action.type) {
        case Actions.ACTION_SET_TRACKERS:

            const newArray = [...action.payload];
            return {
                ...state,
                trackers: newArray
            };
        
        case Actions.ACTION_REMOVE_TRACKER:

            // Remove tracker with id in payload:
            const trackers = state.trackers
                .filter(t => t.id != action.payload);

            // Return new state:
            return {
                ...state,
                trackers: [...trackers]
            };

        case Actions.ACTION_CHANGE_STATUS:

            const cons = state.connections;
            const conEntry = {
                status: event.data[0],
                lastConnection: event.data.length > 1 ? event.data[1] : ""
            };
            cons[event.source] = conEntry;

            // Return new State:
            return {
                ...state,
                connections: { ...cons }
            };

        case Actions.ACTION_UPDATE_LOCATION:

            const locUpdates = state.locationUpdates;
            const loc = {
                latitude: Number(event.data[0]),
                longitude: Number(event.data[1]),
                altitude: Number(event.data[2]),
                speed: Number(event.data[3]),
                direction: Number(event.data[4]),
                battery: Number(event.data[5])
            };
            locUpdates[event.source] = loc;

            // Return new state:
            return {
                ...state,
                locationUpdates: {...locUpdates}
            };
            
        default:
            return state;
    }
};

export default mainReducer;