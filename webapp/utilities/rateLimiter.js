import { RateLimiterMemory } from 'rate-limiter-flexible';


const points = 10;
const duration = 60;

const rateLimiter = new RateLimiterMemory({
  points: points,
  duration: duration,
});


/**
 * Express middleware to handle rate limiting.
 * @param {object} req - Express request object.
 * @param {object} res - Express response object.
 * @param {function} next - Express next middleware function.
 */
const rateLimitMiddleware = (req, res, next) => {
  rateLimiter.consume(req.ip)
    .then(() => {
      next();
    })
    .catch(() => {
      res.status(429).send('Too Many Requests');
    });
};

export default rateLimitMiddleware;
