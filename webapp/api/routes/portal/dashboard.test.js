import "../../../jest.config.js"

require('dotenv-json')();

const request = require('supertest');
// const app = require('../../../server.js');
describe('Request received', () => {
    it('should allow the first request', async () => {
        const res = await request('http://localhost:3000').get('/portal/dashboard'); 

        console.log(res.statusCode); // for debugging

        expect(res.statusCode).not.toBe(400);
    });
    it('should allow the first request', async () => {
        const res = await request('http://localhost:3000').get('/portal/dashboard');

        console.log(res.statusCode); // for debugging

        expect(res.statusCode).toBe(200);
    });
    
});

describe('Rate limit exceeded', () => {
    const baseUrl = 'http://localhost:3000';
    const path = '/portal/dashboard';

    it('should return 429 when limit is exceeded', async () => {
        const maxRequests = 5 ; 
        let response;

        for (let i = 0; i < maxRequests; i++) {
            response = await request(baseUrl)
                .get(path)
                .redirects(0);
            console.log(`Request #${i + 1} - Status: ${response.statusCode}`);
        }

        expect(response.statusCode).toBe(429);
    });
});
