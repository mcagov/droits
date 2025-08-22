import { RateLimiterMemory } from 'rate-limiter-flexible';


const points = 25;
const duration = 1;

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
  rateLimiter.consume(req.ip, 1)
    .then(() => {
      next();
    })
    .catch(() => {
      res.status(429).send('Too Many Requests');
    });
};

export default rateLimitMiddleware;
