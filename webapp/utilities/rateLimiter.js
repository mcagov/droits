import { RateLimiterMemory } from 'rate-limiter-flexible';
import config from "../app/config";

const POINTS = config.RATE_LIMIT_POINTS ? config.RATE_LIMIT_POINTS : 120;
const DURATION = config.RATE_LIMIT_DURATION ? config.RATE_LIMIT_DURATION : 60;

const rateLimiter = new RateLimiterMemory({
  points: POINTS,
  duration: DURATION,
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
