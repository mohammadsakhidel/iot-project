import { createStore } from 'redux';
import mainReducer from './reducers/main-reducer';

export default createStore(mainReducer);