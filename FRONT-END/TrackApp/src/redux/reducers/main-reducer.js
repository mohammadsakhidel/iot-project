import * as Actions from '../actions';

const initialState = {
    trackers: []
};

const mainReducer = (state = initialState, action) => {

    switch (action.type) {
        case Actions.ACTION_SET_TRACKERS:
            const newArray = [...action.payload];
            return {
                ...state,
                trackers: newArray
            };
        case Actions.ACTION_CHANGE_STATUS:
            const event = action.payload;
            console.log(event);
            const trackers = [...state.trackers];

            // Find & Update Tracker:
            const tracker = trackers.find(t => t.id == event.source);
            if (!tracker)
                return state;
            tracker.status = event.data[0];
            tracker.lastConnection = event.data.length > 1 ? event.data[1] : "";

            // Return new State:
            return {
                ...state,
                trackers
            };
        default:
            return state;
    }
};

export default mainReducer;