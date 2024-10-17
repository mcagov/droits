import request from 'supertest';
import app from './server';

describe('GET /', () => {
    it('responds with status 200', (done) => {
        request(app).get('/').expect(200)
            .end((err, res) => {
                if (err) {
                    return done(err);
                }
                return done();
            });
    });
});