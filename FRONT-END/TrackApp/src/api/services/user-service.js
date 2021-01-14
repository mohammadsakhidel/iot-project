import http from '../helpers/http-client';
import * as ApiSettings from '../api-settings.json';
import { StatusCodes } from 'http-status-codes';
import { Strings } from '../../i18n/strings';

export default class UserService {

    static getAvatarUrl(user) {
        return `https://www.gravatar.com/avatar/${user.emailHash.toLowerCase()}?s=200&d=identicon`;
    }

    static async find(token, userNameOrEmail) {
        try {

            const url = `${ApiSettings.BaseUrl}/users/info/${userNameOrEmail}`;
            let response = await http(token).get(url);
            return { done: true, data: response.data };

        } catch (e) {
            if (e.response && e.response.status == StatusCodes.NOT_FOUND)
                return { done: false, data: Strings.UserNotFound };
            throw e;
        }
    }

}