// Constant Action Names:
export const ACTION_SET_TRACKERS = "SET_TRACKERS";

// Functions Returning Action Objects:
export const setTrackers = (trackers) => ({
    type: ACTION_SET_TRACKERS,
    payload: trackers
});