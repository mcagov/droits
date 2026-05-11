jest.mock('redis', () => jest.requireActual('redis-mock'));

process.env.CSRFT_SESSION_SECRET = process.env.CSRFT_SESSION_SECRET || 'test-secret';