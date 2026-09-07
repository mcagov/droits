import request from 'supertest';
import app from './server';

// Mock the config module
jest.mock('./app/config', () => ({
    SERVICE_NAME: 'Report Wreck Material',
    PORT: '3000',
    USE_HTTPS: 'false',
    COOKIE_TEXT:
        'GOV.UK uses cookies to make the site simpler. <a href="#">Find out more about cookies</a>',
    SERVICE_UNAVAILABLE: true,
    RATE_LIMIT_POINTS: 100, // to allow tests passing
}));
    
describe('When config.SERVICE_UNAVAILABLE is true', () => {

    const testRoutes = [
        // Valid routes
        '/',
        '/report/removed-property-check',
        '/portal/login',

        // Invalid route
        '/test-route',

        // Routes with dots
        '/some.route',
        '/file.js',
        '/folder/index.html'
    ]
    
    afterAll(() => {
        jest.resetModules(); // Reset modules to ensure the original config is used for other tests
    });
    
    for (const route of testRoutes) {
        it('should return 503', async () => {
            const res = await request(app).get(route);
            try {
                expect(res.statusCode).toEqual(503);
            } catch (err) {
                throw new Error(`${err.matcherResult.message}\nRoute: ${route}`)
            }
        });
    
        it('should return Service Unavailable in the response body', async () => {
            const res = await request(app).get(route);
            try {
                expect(res.text).toMatch(/Service Unavailable/gi);
            } catch (err) {
                throw new Error(`${err.matcherResult.message}\nRoute: ${route}`)
            }
        });
    
        it('should return the ROW contact email in the response body', async () => {
            const res = await request(app).get(route);
            try {
                expect(res.text).toMatch(/row@mcga.gov.uk/gi);
            } catch (err) {
                throw new Error(`${err.matcherResult.message}\nRoute: ${route}`)
            }
        });
    }
});
