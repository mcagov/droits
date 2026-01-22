process.env.B2C_CLIENT_ID = 'test-client-id';
process.env.ENV_BASE_URL = 'http://localhost:3000';
process.env.B2C_REDIRECT_URL = '/redirect';

jest.mock('../api/auth/authProvider', () => {
    return jest.fn(() => ({
        login: jest.fn(() => (req, res, next) => {
            req.session.isAuthenticated = true;
            req.session.account = {
                idTokenClaims: { oid: 1, username: 'testuser' }
            };
            next();
        }),
    }));
});
