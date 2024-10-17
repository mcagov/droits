import request from 'supertest';
import app from './server';
import config from './app/config';

describe('GET /', () => {
    it('responds with status 200', () => {
        request(app).get('/').expect(200)
        .end((err) => {
            if (err) throw err;
        });
    });
});

describe('Service Shutter Tests', () => {
    
    describe('SERVICE_UNAVAILABLE is true', () => {
        beforeAll(() => {
            jest.mock('./app/config');
            config.SERVICE_UNAVAILABLE = true;
        });
        
        afterAll(() => {
            jest.restoreAllMocks();
        });
        
        it('should return 503', () => {
            request(app).get('/test-route').expect(503)
            .end((err) => {
                if (err) throw err;
            });
        });
    });
});