import request from 'supertest';
import http from 'http';
import {app, redisClient} from './server';

let server;

beforeAll((done) => {
    // Start the server before running any tests
    server = http.createServer(app);  // Create an http server using the Express app
    server.listen(done);              // Start listening for connections
});


afterAll((done) => {
    // Close the server after tests are finished
    redisClient.quit();
    server.close(done);             // Close the server to free up the port
});

describe('GET /', () => {
    it('responds with status 200', async () => {
        const res = await request(app).get('/');
        expect(res.statusCode).toEqual(200);
    });
});