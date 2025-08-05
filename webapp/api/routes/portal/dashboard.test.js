import "../../../jest.config.js"

require('dotenv-json')();

const request = require('supertest');
const app = require('../../../server.js');
const nock = require('nock'); //use nock to mock HTTP calls


beforeEach(() => {
    nock.cleanAll(); // reset all mocks
});
describe('Request received', () => {
    it('should respond with a status code other than 400', async () => {
        const res = await request('http://localhost:3000').get('/portal/dashboard');

        expect(res.statusCode).not.toBe(400);
    });
    it('should respond with a 200 OK status', async () => {
        nock('http://localhost:3000') // set up a mock server
            .get('/portal/dashboard') // <- intercept this GET request
            .reply(200, { success: true }); // <- fake a 200 OK response with a body

        const res = await request('http://localhost:3000').get('/portal/dashboard');
        
        expect(res.statusCode).toBe(200);
    });
    
});

describe('Rate limit exceeded', () => {
    const baseUrl = 'http://localhost:3000';
    const path = '/portal/dashboard';

    it('should return 429 when limit is exceeded', async () => {
        for (let i = 0; i < 5; i++) { // mock the first 5 requests to return 200
            nock(baseUrl)
                .get(path)
                .reply(200, { ok: true });
        }
        
        nock(baseUrl) //  Mock the 6th request to return 429
            .get(path)
            .reply(429, { error: 'Too Many Requests' });
        
        //     const maxRequests = 5 ; 
        let response;

        for (let i = 0; i <= 5; i++) {
            response = await request(baseUrl)
                .get(path)
                .redirects(0);
            console.log(`Request #${i + 1} - Status: ${response.statusCode}`);
        }

        expect(response.statusCode).toBe(429);
    });
});

import axios from 'axios';
// HTTP client library for making HTTP requests (GET, POST, etc.) from Node.js
describe('API call test', () => {
    test('should mock GET /here and return expected data', async () => {
        const baseUrl = 'http://mockapi.com';

        // Mock GET /here to respond with { msg: 'there' }
        nock(baseUrl)
            .get('/here')
            .reply(200, { msg: 'there' });

        // Make actual axios call to the mocked URL
        const response = await axios.get(`${baseUrl}/here`);

        // Test that the mocked response returns what we expect
        expect(response.status).toBe(200);
        expect(response.data.msg).toBe('there');  
    });
});