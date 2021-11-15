import AsyncStorage from '@react-native-async-storage/async-storage';

const ITEM_USER = '@user';
const ITEM_TRACKERS = '@trackers';

export function saveUserAsync(user) {
    return AsyncStorage.setItem(ITEM_USER, JSON.stringify(user));
}

export function removeUserAsync() {
    return AsyncStorage.removeItem(ITEM_USER);
}

export async function getUserAsync() {
    let json = await AsyncStorage.getItem(ITEM_USER);
    if (!json)
        return null;

    return JSON.parse(json);
}

export async function saveTrackerInfoAsync(trackerId, trackerInfo) {

    let json = await AsyncStorage.getItem(ITEM_TRACKERS);
    let trackers = json ? JSON.parse(json) : {};

    if (trackerInfo && trackerId)
        trackers[trackerId] = trackerInfo;

    await AsyncStorage.setItem(ITEM_TRACKERS, JSON.stringify(trackers));

}

export async function getTrackerInfoAsync(trackerId) {

    let json = await AsyncStorage.getItem(ITEM_TRACKERS);
    let trackers = json ? JSON.parse(json) : null;

    return trackers ? trackers[trackerId] : null;
}