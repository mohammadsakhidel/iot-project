export const ACTION_INC = "INC";
export const ACTION_DEC = "DEC";

export const getInc = (count) => {
    return {
        type: ACTION_INC,
        payload: count
    };
}

export const getDec = (count) => {
    return {
        type: ACTION_DEC,
        payload: count
    };
}