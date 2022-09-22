import simpleRestProvider from 'ra-data-simple-rest';
import { fetchUtils } from 'react-admin';

const apiLocation = process.env.REACT_APP_DATA_API || '/api';

const httpClient = (url: string, options: any = {}) => {
    if (!options.headers) {
        options.headers = new Headers({ Accept: 'application/json' });
    }

    const token = localStorage.getItem('access_token');
    if (token) {
        options.headers.set('Authorization', 'Bearer ' + token);
    }

    return fetchUtils.fetchJson(url, options).catch(response => {
        if (response.status === 401) {
            localStorage.removeItem('access_token');
            localStorage.removeItem('userid');
            localStorage.removeItem('username');
            localStorage.removeItem('fullname');
            localStorage.removeItem('avatar');
        }
        return response;
    });
};

const restProvider = simpleRestProvider(apiLocation, httpClient);

export default restProvider;
