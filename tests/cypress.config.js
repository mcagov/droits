const { defineConfig } = require("cypress");

module.exports = defineConfig({
    requestTimeout: 2000, 
    defaultCommandTimeout: 5000,
    retries: 3,
  e2e: {
    experimentalStudio: true,
    setupNodeEvents(on, config) {
      // implement node event listeners here
      
    },
    baseUrl: "http://127.0.0.1:3000", 
    supportFile: 'e2e.js',
  },
});
