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
        default:
            return state;
    }
};

export default mainReducer;