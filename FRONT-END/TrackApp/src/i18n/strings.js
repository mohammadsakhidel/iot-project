export const Strings = require('./en.json');

export const getString = (key) => {
    if (key in Strings)
        return Strings[key];
    else
        return '';
}