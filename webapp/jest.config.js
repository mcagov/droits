module.exports = {
    setupFilesAfterEnv: [
        './test-mocks/jest.setup.redis-mock.js', 
        './test-mocks/jest.setup.passport-mock.js',
        './test-mocks/jest.setup.passport-azure-ad.js'
    ],
};