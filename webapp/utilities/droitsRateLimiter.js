const { RateLimiterMemory } = require("rate-limiter-flexible");

const POINTS = config.RATE_LIMIT_POINTS ? config.RATE_LIMIT_POINTS : 10;
const DURATION = config.RATE_LIMIT_DURATION ? config.RATE_LIMIT_DURATION : 60;

// const MAX_REQUESTS = 10;
// const WINDOW_MS = 60 * 1000;

const rateLimiter = new RateLimiterMemory({
    points : MAX_REQUESTS,
    duration : WINDOW_MS / 1000,
});

/**
 * Express middleware to handle rate limiting.
 * @param {object} req - Express request object.
 * @param {object} res - Express response object.
 * @param {function} next - Express next middleware function.
 */

const rateLimitMiddleware = (req, res, next) => {
    rateLimiter.consume(req.ip, 1)
        .then(() => {
            next();
        })
        .catch(() => {
            res.status(429).send('Too Many Requests');
        });
};

export default rateLimitMiddleware;
// export default rateLimiter;

