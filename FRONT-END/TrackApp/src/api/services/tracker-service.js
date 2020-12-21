import http from '../helpers/http-client';
import * as ApiSettings from '../api-settings.json';

export default class TrackerService {



    static async list(token) {
        try {

            const url = `${ApiSettings.BaseUrl}/trackers/mine`;
            const resp = await http(token).get(url);
            return { done: true, data: resp.data };

        } catch (e) {
            throw e;
        }
    }

    static async remove(trackerId, token) {
        try {

            const url = `${ApiSettings.BaseUrl}/trackers/${trackerId}/user`;
            const resp = await http(token).delete(url);
            return { done: true, data: resp.data };

        } catch (e) {
            throw e;
        }
    }

    static getIconUrl(tracker) {
        return `${ApiSettings.BaseUrl}/images/${tracker.iconImageId ?? 'defaulticon'}?d=${tracker.productType}`;
    }

}