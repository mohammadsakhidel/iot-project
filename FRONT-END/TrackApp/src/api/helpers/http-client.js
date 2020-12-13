import axios from 'axios';

export default function httpClient(token) {
    if (token) {
        axios.defaults.headers.common['Authorization'] = `Bearer ${token}`;
    }

    return axios;
}