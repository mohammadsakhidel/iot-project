import * as data from './en.json';

export default class Strings {

    static get(key) {
        if (key in data)
            return data[key];
        else
            return '';
    }
    
}