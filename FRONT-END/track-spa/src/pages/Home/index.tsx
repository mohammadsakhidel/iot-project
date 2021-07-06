import React from 'react';
import { useTranslation, withTranslation } from 'react-i18next';

function Home({ t }: any) {
    return (
        <div>
            {t('welcome')}
        </div>
    )
}

export default withTranslation()(Home);
