// Constant Action Names:
export const ACTION_SET_TRACKERS = "SET_TRACKERS";
export const ACTION_REMOVE_TRACKER = "REMOVE_TRACKER";
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

export const removeTracker = (trackerId) => ({
    type: ACTION_REMOVE_TRACKER,
    payload: trackerId
});