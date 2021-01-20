export const devicePasswordValidator = (value) => {
    const regex = /^[a-zA-Z0-9]{6,}$/;
    return regex.test(value);
};

export const phoneNumberValidator = (value) => {
    const regex = /^\+?\d{10,14}$/;
    return regex.test(value);
};

export const numberValidator = (value) => {
    const regex = /^\d+$/;
    return regex.test(value);
};