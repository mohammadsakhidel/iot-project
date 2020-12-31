import http from '../helpers/http-client';
import * as ApiSettings from '../api-settings.json';

export default class PollingService {

    static async poll(token, pollingInput) {

        const url = `${ApiSettings.BaseUrl}/polling`;
        const resp = await http(token).post(url, pollingInput);
        return { done: true, data: resp.data };

    }

}