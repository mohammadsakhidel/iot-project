import * as ApiSettings from '../api-settings.json';
import http from '../helpers/http-client';

export default class EventsService {

    /**
     * 
     * @returns {{done: Boolean, data: { accessCode: String, servers: Object[] }}}
     */
    static async getConnectionInfoAsync(token) {

        const url = `${ApiSettings.BaseUrl}/events/accesscode`;
        const resp = await http(token).get(url);
        return { done: true, data: resp.data };

    }

}