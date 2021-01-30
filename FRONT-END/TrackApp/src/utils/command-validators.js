import * as Patterns from '../constants/patterns';

export const devicePasswordValidator = (value) => {
    const regex = Patterns.DEVICE_PASSWORD;
    return regex.test(value);
};

export const phoneNumberValidator = (value) => {
    const regex = Patterns.PHONE_NUMBER;
    return regex.test(value);
};

export const numberValidator = (value) => {
    const regex = Patterns.NUMBER;
    return regex.test(value);
};