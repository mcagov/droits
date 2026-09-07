jest.mock('redis', () => jest.requireActual('redis-mock'));
jest.mock('dotenv-json', () => () => {});

process.env.CSRFT_SESSION_SECRET = process.env.CSRFT_SESSION_SECRET || 'test-secret';
process.env.B2C_BASE_URL = process.env.B2C_BASE_URL || 'https://testb2cmcga.b2clogin.com/TESTB2CMCGA.onmicrosoft.com';
process.env.ENV_BASE_URL = process.env.ENV_BASE_URL || 'http://localhost:3000';
