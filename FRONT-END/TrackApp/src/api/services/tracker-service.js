import http from '../helpers/http-client';
import * as ApiSettings from '../api-settings.json';
import { StatusCodes } from 'http-status-codes';
import { Strings } from '../../i18n/strings';
import * as ErrorCodes from '../../constants/error-codes';

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

    static async add(trackerId, token) {
        try {

            const url = `${ApiSettings.BaseUrl}/trackers/${trackerId}/user`;
            const resp = await http(token).put(url);
            return { done: true, data: resp.data };

        } catch (e) {
            if (e.response && e.response.status == StatusCodes.NOT_FOUND)
                return { done: false, data: Strings.DeviceNotFound };
            else if (e.response && e.response.status == StatusCodes.BAD_REQUEST
                && e.response.data == ErrorCodes.USER_ASSIGNED)
                return { done: false, data: Strings.UserPreviouslyAssigned };

            throw e;
        }
    }

    static getIconUrl(tracker) {
        return `${ApiSettings.BaseUrl}/images/${tracker.iconImageId ?? 'defaulticon'}?d=${tracker.productType}`;
    }

}