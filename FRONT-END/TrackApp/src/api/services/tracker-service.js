import http from '../helpers/http-client';
import * as ApiSettings from '../api-settings.json';

export default class TrackerService {



    static async list(token) {
        try {

            const url = `${ApiSettings.BaseUrl}/trackers/mine`;
            const resp = await http(token).get(url);
            return { done: true, data: resp.data };

        } catch (e) {
            // return e.response
            //     ? { done: false, data: e.response.data }
            //     : { done: false, data: e.message };
            return { done: false, data: e.message };
        }
    }

    static getIconUrl(tracker) {
        return `${ApiSettings.BaseUrl}/images/${tracker.iconImageId ?? 'defaulticon'}?d=${tracker.productType}`;
    }

}