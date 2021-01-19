export const devicePasswordValidator = (value) => {
    const regex = /^[a-zA-Z0-9]{6,}$/;
    return regex.test(value);
};