import jwtDecode from 'jwt-decode';
import * as ClaimNames from '../constants/claim-names';

export default class AppUser {
    constructor() {
        this.id = '';
        this.givenName = '';
        this.surname = '';
        this.token = '';
    }

    static parseToken(jwtToken) {
        const jwtObject = jwtDecode(jwtToken);

        console.log(jwtObject);

        appUser = new AppUser();
        appUser.token = jwtToken;
        appUser.id = jwtObject[ClaimNames.CLAIM_USERID];
        appUser.givenName = jwtObject[ClaimNames.CLAIM_GIVEN_NAME];
        appUser.surname = jwtObject[ClaimNames.CLAIM_SURNAME];

        return appUser;
    }
}