import jwtDecode from 'jwt-decode';
import * as ClaimNames from '../constants/claim-names';

export default class AppUser {
    constructor() {
        this.id = '';
        this.givenName = '';
        this.surname = '';
        this.token = '';
        this.emailHash = '';
    }

    static parseToken(jwtToken) {
        const jwtObject = jwtDecode(jwtToken);

        appUser = new AppUser();
        appUser.token = jwtToken;
        appUser.id = jwtObject[ClaimNames.CLAIM_USERID];
        appUser.givenName = jwtObject[ClaimNames.CLAIM_GIVEN_NAME];
        appUser.surname = jwtObject[ClaimNames.CLAIM_SURNAME];
        appUser.emailHash = jwtObject[ClaimNames.CLAIM_EMAIL_HASH];

        return appUser;
    }
}