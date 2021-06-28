import * as Actions from './actions';

const initialState = {
    counter: 0
};

const mainReducer = (state = initialState, action) => {
    switch (action.type) {
        case Actions.ACTION_INC:
            return {
                ...state,
                counter: state.counter + action.payload
            };
        case Actions.ACTION_DEC:
            return {
                ...state,
                counter: state.counter - action.payload
            };
        default:
            return state;
    }
};

export default mainReducer;