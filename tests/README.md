# Testing

This directory contains system-level tests for **droits**.

## End-to-End Testing

Automated end-to-end tests are implemented using [Cypress](https://www.cypress.io/).  

To run the tests locally, use the following commands:

```bash
docker compose up
npm run test:e2e
```

## Smoke Testing

Smoke tests live in `cypress/e2e/smoke` and run against a deployed environment after each deployment.

The signed-in tests only run in development (for now) and need a test account. To run them locally, add the account to `cypress.env.json` (git ignored):

```json
{
  "TEST_USER_EMAIL": "",
  "TEST_USER_PASSWORD": ""
}
```

Then run:

```bash
CYPRESS_BASE_URL=https://dev.report-wreck-material.service.gov.uk CYPRESS_ENVIRONMENT=development npx cypress open --e2e --browser electron
```

In GitHub, the account is stored as `TEST_USER_EMAIL` and `TEST_USER_PASSWORD` secrets on the `development` environment.
