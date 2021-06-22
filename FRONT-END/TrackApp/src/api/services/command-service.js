import http from '../helpers/http-client';
import * as ApiSettings from '../api-settings.json';
import { StatusCodes } from 'http-status-codes';
import { Strings } from '../../i18n/strings';
import * as ErrorCodes from '../../constants/error-codes';

export default class CommandService {

    /**
     * 
     * @param {{trackerId: string, commandType: string, payload: string}} dto 
     * @param {String} token
     * @returns {{done: boolean, data: string}}
     */
    static async execute(dto, token, endpoint) {

        const url = `${ApiSettings.BaseUrl}/commands${(endpoint ? `/${endpoint}` : '')}`;

        //console.log(url);
        //console.log(dto);
        //console.log(token);

        const response = await http(token).post(url, dto);
        const apiResult = response.data;

        return { done: apiResult.done, data: apiResult.done ? apiResult.data : apiResult.error };

    }

    static async getContacts(trackerId, token) {

        const url = `${ApiSettings.BaseUrl}/commands/${trackerId}/contacts`;
        const response = await http(token).get(url);
        const items = response.data.filter(c => c.name && c.number);
        return { done: true, data: items };

    }

    static async removeContact(trackerId, number, token) {
        const url = `${ApiSettings.BaseUrl}/commands/${trackerId}/contacts?number=${encodeURIComponent(number)}`;
        const response = await http(token).delete(url);
        return { done: true, data: response.data };
    }

    static async addContact(dto, token) {
        try {

            const url = `${ApiSettings.BaseUrl}/commands/${dto.trackerId}/contacts`;

            const response = await http(token).post(url, dto);

            return { done: true, data: response.data };

        } catch (e) {
            if (e.response && e.response.status == StatusCodes.BAD_REQUEST
                && e.response.data == ErrorCodes.ALREADY_ADDED)
                return { done: false, data: Strings.ContactAlreadyAdded };
            else if (e.response && e.response.status == StatusCodes.BAD_REQUEST
                && e.response.data == ErrorCodes.CONTACTS_FULL)
                return { done: false, data: Strings.ContactsFullError };
            else if (e.response && e.response.status == StatusCodes.BAD_REQUEST
                && e.response.data == ErrorCodes.NOT_ALLOWED)
                return { done: false, data: Strings.AccessDeniedError };
            else
                throw e;
        }
    }


    static async getConfigs(trackerId, token) {

        const url = `${ApiSettings.BaseUrl}/trackers/${trackerId}/configs`;

        const response = await http(token).get(url);

        return { done: true, data: response.data };

    }

}