import request from 'supertest';
import app from './server';

describe('GET /', () => {
    it('responds with status 200', async () => {
        const res = await request(app).get('/');
        expect(res.statusCode).toEqual(200);
    });
});