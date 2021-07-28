import moment from 'moment';
import * as Formats from '../constants/formats';
import { Strings } from '../i18n/strings';

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

export function getRepeatString(repeat) {

    const repetitionStringToDayNames = (repStr) => {
        const array = [];
        if (!repStr || repStr.length < 7)
            return '';

        if (repStr.charAt(0) == '1')
            array.push(Strings.Monday);
        if (repStr.charAt(1) == '1')
            array.push(Strings.Tuesday);
        if (repStr.charAt(2) == '1')
            array.push(Strings.Wednesday);
        if (repStr.charAt(3) == '1')
            array.push(Strings.Thursday);
        if (repStr.charAt(4) == '1')
            array.push(Strings.Friday);
        if (repStr.charAt(5) == '1')
            array.push(Strings.Saturday);
        if (repStr.charAt(6) == '1')
            array.push(Strings.Sunday);

        return array.join(', ');
    };

    return (!repeat || repeat == '0000000') ? Strings.Once : (repeat == '1111111' ? Strings.EveryDay : repetitionStringToDayNames(repeat))
}

export function format(inputText, ...args) {
    return inputText.replace(/{(\d+)}/g, function (match, number) {
        return typeof args[number] != 'undefined'
            ? args[number]
            : match
            ;
    });
}