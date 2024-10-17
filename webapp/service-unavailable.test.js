import request from 'supertest';
import app from './server';

// Mock the config module
jest.mock('./app/config', () => ({
    SERVICE_NAME: 'Report Wreck Material',
    PORT: '3000',
    USE_HTTPS: 'false',
    COOKIE_TEXT:
        'GOV.UK uses cookies to make the site simpler. <a href="#">Find out more about cookies</a>',
    SERVICE_UNAVAILABLE: true
}));

const testRoutes = [
    // Valid routes
    '/',
    '/report/removed-property-check',
    '/portal/login',

    // Invalid route
    '/test-route'
]
    
describe('When config.SERVICE_UNAVAILABLE is true', () => {
    afterAll(() => {
        jest.resetModules(); // Reset modules to ensure the original config is used for other tests
    });
    
    it('should return 503', (done) => {
        for (const route of testRoutes) {
            request(app).get(route)
                .expect(503)
                .end((err, res) => {
                    if (err) {
                        console.log(route, res.statusCode)
                        return done(err);
                    }
                    return done();
                });
        }
    });

    it('should return Service Unavailable in the response body', (done) => {
        for (const route of testRoutes) {
            request(app).get(route)
                .expect(/Service Unavailable/gi)
                .end((err, res) => {
                    if (err) {
                        console.log(route, res.statusCode)
                        return done(err);
                    }
                    return done();
                });
        }
    });

    it('should return the ROW contact email in the response body', (done) => {
        for (const route of testRoutes) {
            request(app).get(route)
                .expect(/row@mcga.gov.uk/gi)
                .end((err, res) => {
                    if (err) {
                        console.log(route, res.statusCode)
                        return done(err);
                    }
                    return done();
                });
        }
    });
});