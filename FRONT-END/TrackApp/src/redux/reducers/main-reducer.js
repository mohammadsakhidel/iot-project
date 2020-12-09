import * as Actions from '../actions';

const initialState = {
    counter: 1
};

const mainReducer = (state = initialState, action) => {
    switch (action.type) {
        case Actions.ACTION_INC_COUNTER:
            return Object.assign({}, state, { counter: state.counter + 1 });
        default:
            return state;
    }
};

export default mainReducer;