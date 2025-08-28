import request from 'supertest';
import app from './server';

jest.mock('./app/config', () => ({
    SERVICE_NAME: 'Report Wreck Material',
    PORT: '3000',
    USE_HTTPS: 'false',
    COOKIE_TEXT:
        'GOV.UK uses cookies to make the site simpler. <a href="#">Find out more about cookies</a>',
    RATE_LIMIT_POINTS: 1,
    RATE_LIMIT_DURATION: 60,
}));
    
describe('When RATE_LIMIT_POINTS are exceeded', () => {

    afterAll(() => {
        jest.resetModules();
    });
    
    const testRoute = '/'; 
    
    it('should return 429 after the first request', async () => {
        try {
            const firstRes = await request(app).get(testRoute);
            expect(firstRes.statusCode).not.toEqual(429);
            
            const secondRes = await request(app).get(testRoute);
            expect(secondRes.statusCode).toEqual(429);

        } catch (err) {
            throw new Error(`${err.matcherResult.message}\nRoute: ${testRoute}`);
        }
    });
});
