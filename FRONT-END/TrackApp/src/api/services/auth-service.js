import * as ApiSettings from '../api-settings.json';
import axios from 'axios';
import { StatusCodes } from 'http-status-codes';
import { Strings } from '../../i18n/strings';

export default class AuthService {
    static async login(dto) {
        try {

            const url = `${ApiSettings.BaseUrl}/auth/token`;
            let response = await axios.post(url, dto);
            return { done: true, data: response.data };

        } catch (e) {
            if (e.response && e.response.status == StatusCodes.BAD_REQUEST)
                return { done: false, data: Strings.InvalidCredentials };
            throw e;
        }
    }
}