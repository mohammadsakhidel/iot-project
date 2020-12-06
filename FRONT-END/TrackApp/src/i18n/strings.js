import memoize from 'lodash.memoize'; // Use for caching/memoize for better performance
import i18n from 'i18n-js';
import * as Localization from 'expo-localization';
import { I18nManager } from 'react-native';

export const translationGetters = {
    'en-US': () => require('./en.json'),
    'fr-FR': () => require('./fr.json'),
};

export const Strings = memoize(
    (key, config) =>
        i18n.t(key, config).includes('missing') ? key : i18n.t(key, config),
    (key, config) => (config ? key + JSON.stringify(config) : key),
);

Strings.init = () => {
    let localeLanguageTag = 'en-US';    // Localization.locale;
    let isRTL = false;                  // Localization.isRTL;

    Strings.cache.clear();

    // update layout direction
    I18nManager.forceRTL(isRTL);

    // set i18n-js config
    i18n.translations = {
        [localeLanguageTag]: translationGetters[localeLanguageTag](),
    };
    i18n.locale = localeLanguageTag;
};