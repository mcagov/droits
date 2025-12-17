jest.mock('passport', () => ({
    use: jest.fn((strategy, callback) => {}),

    authenticate: jest.fn((strategy, options) => (req, res, next) => {
        // Simulate a successful authentication for 'local' strategy
        if (strategy === 'azuread-openidconnect') {
            req.user = { id: 1, username: 'testuser' }; // Simulate a user object
            return next();
        }
        // Simulate a failed authentication
        res.status(401).send({ message: 'Authentication failed' });
    }),

    initialize: jest.fn(() => (req, res, next) => next()),

    session: jest.fn(() => (req, res, next) => next()),
    
    serializeUser: jest.fn((user, done) => {
        if (typeof done === 'function') {
            done(null, user.id); // call done without errors, passing user.id
        }
    }),

    deserializeUser: jest.fn((id, done) => {
        const user = { id, username: 'testuser' }; // Simulate fetching a user by id
        if (typeof done === 'function') {
            done(null, user); // call done without errors, passing user object
        }
    })
}));