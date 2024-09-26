import request from 'supertest';
import app from './server';

describe('GET /', () => {
    it('responds with status 200', () => {
        request(app).get('/').expect(200)
        .end((err) => {
            if (err) throw err;
        });
    });
});