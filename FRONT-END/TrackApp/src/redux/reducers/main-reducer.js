import * as Actions from '../actions';

const initialState = {
    dialogResult: { key: '', value: '' }
};

const mainReducer = (state = initialState, action) => {
    switch (action.type) {
        case Actions.ACTION_SET_DIALOG_RESULT:
            return Object.assign({}, state, { dialogResult: action.payload });
        default:
            return state;
    }
};

export default mainReducer;