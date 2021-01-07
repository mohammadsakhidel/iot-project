export default class UserService {

    static getAvatarUrl(user) {
        return `https://www.gravatar.com/avatar/${user.emailHash.toLowerCase()}?s=200&d=identicon`;
    }

}