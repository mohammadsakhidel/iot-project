import * as Actions from '../actions';

const initialState = {
    trackers: [],
    connections: {}
};

const mainReducer = (state = initialState, action) => {

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

            const event = action.payload;
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
            
        default:
            return state;
    }
};

export default mainReducer;