import moment from 'moment';
import * as Formats from '../constants/formats';

export function getRandom(length) {
    var result = '';
    var characters = 'ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789';
    var charactersLength = characters.length;
    for (var i = 0; i < length; i++) {
        result += characters.charAt(Math.floor(Math.random() * charactersLength));
    }
    return result;
}

export function formatDateTime(dateTime) {
    return moment(dateTime).format(Formats.DISPLAY_DATETIME_FORMAT);
}

export function formatDateString(dateString) {
    const dt = new Date(Date.parse(dateString));
    return formatDateTime(dt);
}