// Constant Action Names:
export const ACTION_SET_TRACKERS = "SET_TRACKERS";
export const ACTION_CHANGE_STATUS = "CHANGE_STATUS";

// Functions Returning Action Objects:
export const setTrackers = (trackers) => ({
    type: ACTION_SET_TRACKERS,
    payload: trackers
});

/**
 * 
 * @param {{name: String, source: String, data: String}} event 
 */
export const changeTrackerStatus = (event) => ({
    type: ACTION_CHANGE_STATUS,
    payload: event
});