// Constant Action Names:
export const ACTION_SET_DIALOG_RESULT = 'ACTION_SET_DIALOG_RESULT';

// Functions Returning Action Objects:
export const setDialogResult = (result) => ({
    type: ACTION_SET_DIALOG_RESULT,
    payload: result
});