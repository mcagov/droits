jest.mock('passport-azure-ad', () => ({
    OIDCStrategy: jest.fn((options, verify) => {
        return {
            name: 'oidc',
            authenticate: (req, options) => {
                const user = { id: 1, username: 'testuser' }; // Simulate authenticated user
                verify(null, user); // Simulate successful verification
            }
        };
    })
}));