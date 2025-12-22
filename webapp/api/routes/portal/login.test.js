import {deserializeUser, findByOid, serializeUser, users, verifyStrategy} from "./login";
import app from '../../../server';

const request = require('supertest');
const passport = require('passport');


describe('Login Unit Tests', () => {

    beforeEach(() => {
        users.length = 0;
        jest.clearAllMocks();
    });

    describe('findByOid', () => {
        it('should return null if user does not exist', (done) => {
            findByOid('', (err, user) => {
                expect(err).toBeNull();
                expect(user).toBeNull();
                done();
            });
        });

        it('should return the user object if they exist', (done) => {
            const mockUser = { oid: 'user-123', name: 'test-user' };
            users.push(mockUser);

            findByOid('user-123', (err, user) => {
                expect(err).toBeNull();
                expect(user).toEqual(mockUser);
                done();
            });
        });
    });

    describe('serializeUser', () => {
        it('should return the user oid', (done) => {
            const mockUser = { oid: 'user-123' };
            serializeUser(mockUser, (err, id) => {
                expect(err).toBeNull();
                expect(id).toBe('user-123');
                done();
            });
        });
    });

    describe('deserializeUser', () => {
        it('should retrieve full user object from oid', (done) => {
            const mockUser = { oid: 'user-123', email: 'test@example.com' };
            users.push(mockUser);

            deserializeUser('user-123', (err, user) => {
                expect(err).toBeNull();
                expect(user).toEqual(mockUser);
                done();
            });
        });
    });

    describe('verifyStrategy (OIDC Logic)', () => {
        // Mock data for OIDC strategy callback
        const iss = 'issuer';
        const sub = 'subject';
        const accessToken = 'token';
        const refreshToken = 'refresh';
        const params = {};

        it('should error if profile has no OID', (done) => {
            const profileWithoutOid = { };

            verifyStrategy(iss, sub, profileWithoutOid, accessToken, refreshToken, params, (err, user) => {
                expect(err).toBeInstanceOf(Error);
                expect(err.message).toBe('No oid found');
                expect(user).toBeNull();
                done();
            });
        });

        it('should register a new user if they do not exist', (done) => {
            const newProfile = { oid: 'user-123', name: 'test-user' };

            verifyStrategy(iss, sub, newProfile, accessToken, refreshToken, params, (err, user) => {
                expect(err).toBeNull();
                expect(user).toEqual(newProfile);
                
                expect(users).toHaveLength(1);
                expect(users[0]).toEqual(newProfile);
                done();
            });
        });

        it('should return existing user if they already exist', (done) => {
            const existingUser = { oid: 'user-123', name: 'existing-user' };
            users.push(existingUser);

            const incomingProfile = { oid: 'user-123', name: 'existing-userGuy Update' };

            verifyStrategy(iss, sub, incomingProfile, accessToken, refreshToken, params, (err, user) => {
                expect(err).toBeNull();
                expect(user).toEqual(existingUser);
                expect(users).toHaveLength(1);
                done();
            });
        });
    });
});

describe('Login Integration Tests', () => {

    afterEach(() => {
        jest.clearAllMocks();
        jest.restoreAllMocks();
    });
    
    it('GET /login should authenticate using the strategy and render the dashboard', async () => {
        const response = await request(app).get('/login');
        expect(response.status).toBe(200);
        expect(response.text).toContain('Your reports of wreck material');
    });


    it('GET /login should redirect 302 when auth fails', async () => {
        passport.authenticate.mockImplementationOnce((strategy, options) => (req, res) => {
            if (options && options.failureRedirect) {
                return res.redirect(options.failureRedirect);
            }
        });
        
        const response = await request(app).get('/login');
        expect(response.status).toBe(302);
        expect(response.headers.location).toContain('https://testb2cmcga.b2clogin.com/TESTB2CMCGA.onmicrosoft.com/oauth2/v2.0/logout?p=B2C_1_login&post_logout_redirect_uri=');
    });
});