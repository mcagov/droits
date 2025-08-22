const { RateLimiterMemory } = require("rate-limiter-flexible");

const MAX_REQUESTS = 10;
const WINDOW_MS = 60 * 1000;

const rateLimiter = new RateLimiterMemory({
    points : MAX_REQUESTS,
    duration : WINDOW_MS / 1000,
});

export default rateLimiter;

