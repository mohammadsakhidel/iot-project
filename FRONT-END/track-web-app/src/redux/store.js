import { createStore } from 'redux';
import mainReducer from './main-reducer';

export default createStore(mainReducer);