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
    static async execute(dto, token) {

        const url = `${ApiSettings.BaseUrl}/commands`;
        const response = await http(token).post(url, dto);
        const apiResult = response.data;

        return { done: apiResult.done, data: apiResult.done ? apiResult.data : apiResult.error };

    }
}