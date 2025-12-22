import app from '../../../../server';
import {authProvider, users} from "./login";
const request = require('supertest');


// Mock the config module
jest.mock('../../../../app/config', () => ({
    SERVICE_NAME: 'Report Wreck Material',
    PORT: '3000',
    USE_HTTPS: 'false',
    COOKIE_TEXT:
        'GOV.UK uses cookies to make the site simpler. <a href="#">Find out more about cookies</a>',
    SERVICE_UNAVAILABLE: false,
    USE_MSAL: true,
    RATE_LIMIT_POINTS: 100, // to allow tests passing
}));


describe('MSAL Login Tests', () => {
    afterEach(() => {
        jest.clearAllMocks();
        jest.resetAllMocks();
    })

    it('GET /login should authenticate using the strategy and render the dashboard', async () => {
        const response = await request(app).get('/login');
        expect(response.status).toBe(200);
        expect(response.text).toContain('Your reports of wreck material');
    });

    it('GET /login should redirect 302 when auth fails or if session is invalid', async () => {
        authProvider.login.mockImplementation(() => (req, res, next) => {
            req.session.isAuthenticated = false;
            req.session.account = null;
            next();
        });
        const response = await request(app).get('/login');
        expect(response.status).toBe(302);
        expect(response.header.location).toContain('https://testb2cmcga.b2clogin.com/TESTB2CMCGA.onmicrosoft.com/oauth2/v2.0/logout?p=B2C_1_login&post_logout_redirect_uri');
    });
    
});