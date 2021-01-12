import * as ApiSettings from '../api-settings.json';
import http from '../helpers/http-client';

export const getConnectionInfoAsync = async (token) => {

    const url = `${ApiSettings.BaseUrl}/events/accesscode`;
    const resp = await (http(token).get(url));
    return { done: true, data: resp.data };

}