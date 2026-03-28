![CI/CD Pipeline](https://github.com/mcagov/droits/actions/workflows/pipeline-dev.yml/badge.svg)
[![Staging pipeline](https://github.com/mcagov/droits/actions/workflows/pipeline-staging.yml/badge.svg)](https://github.com/mcagov/droits/actions/workflows/pipeline-staging.yml)
[![Production pipeline](https://github.com/mcagov/droits/actions/workflows/pipeline-prod.yml/badge.svg)](https://github.com/mcagov/droits/actions/workflows/pipeline-prod.yml)

# Droits reporting service

The Droits reporting service enables:

- Salvage owners to report their findings with the Maritime & CoastGuard Agency
- Receivers of Wreck to manage these reports

It comprises two applications:

1. A public facing frontend.
   - Source code is in the `webapp/` directory.
   - Application specific documentation is in the [README](./webapp/README.md).
2. The backoffice API which handles the incoming registrations and is an admin application to handle the reports
   - Source code is in the `backoffice/` directory.
   - Application specific documentation is in the [README](./backoffice/README.md).

## Architecture

![Architecture diagram](./architecture.jpg)

Infrastructure-as-code remains the single source-of-truth for Droits infrastructure! Always check `terraform/` if
unsure.

## Local development

| **dependency**                                                    | **version** |
|-------------------------------------------------------------------|-------------|
| [python](https://www.python.org/)                                 | 3.10.17     |
| [dotnet](https://learn.microsoft.com/en-us/dotnet/)               | 8.0.409     |
| [nvm](https://github.com/nvm-sh/nvm)                              | 0.39.5      |
| [node](https://github.com/nvm-sh/nvm)                             | 18.17.0     |
| [terraform](https://www.terraform.io/)                            | 1.4.6       |
| [Docker desktop](https://www.docker.com/products/docker-desktop/) | Latest      |

To install the dependencies, follow [this guide](/docs/dependencies-setup.md)

### Pre-commit hooks

Currently, they just run `terraform fmt` on files staged for commit. This should save you waiting for a pipeline to tell you it needs doing.

Enable the pre-commit hooks by running...

```shell
npm install
npm run prepare
```

### Getting started

To run the application, you'll need the following configuration files:

```bash
webapp/.env.json
backoffice/src/appsettings.json
```

The contents for these files are stored in `1Password`. Please request access from the team if you do not already have it.
Once you have these files in place, you can start the application by running `docker compose up`.

### Troubleshooting

- Instance fails to start: If you ran `docker compose up` before creating and populating the `.env.json` and `appsettings.json` files, this will cause the instance to fail. To resolve this, clean up the environment and run the command again.

- Port 5005 is unavailable: If you encounter a port binding error, port `5005` is already in use. To solve this, run `HOST_PORT=5002 docker compose up` (or alternative port number)

## Infrastructure-as-code

The [Terraform](./terraform) directory contains the Terraform code for managing the infrastructure for the Droits
reporting service.

## Deployment

Automated testing, building and deployment is performed using GitHub Actions with configuration held in
`.github/workflows`.

### Development environment

A build and deployment to the development environment is triggered on each push to `main`. Docker images are tagged
with the hash of the triggering commit and published to AWS Elastic Container Registry. Images built and deployed to
for the development environment are ephemeral and not used anywhere else.

### Staging environment

A build and deployment to the staging environment is triggered on a manual release set to "pre-release". Docker images are tagged
with the hash of the triggering commit and published to AWS Elastic Container Registry. Images built and deployed to
for the staging environment are ephemeral and not used anywhere else.

Please ensure that all staged releases follow semantic versioning: [semver](https://semver.org/). 

### Production environment

A build and deployment to the production environment is triggered on a manual release set to "latest release". Docker images are tagged
with the hash of the triggering commit and published to AWS Elastic Container Registry. Images built and deployed to
for the production environment are ephemeral and not used anywhere else.

### Smoke tests

These are manual at the moment. Writing them down is the first step on the journey towards automating them. There are also several differences between Dev/Staging and Production that will need resolving before we can automate them sensibly.

After deploying:

- **Web App**
  - Check the healthcheck endpoint - It should say "OK"
  - Check the "Report Wreck Material" home page loads
    - Initial pages are currently quite different between Dev/Staging and Production
      - Staging * Dev
        - Check the three help links top right work
        - Click through to "Report Wreck Material"
        - Check the "required format" links triggers download of the CSV template
        - Check the "contact the Receiver of Wreck" link works
      - Production
        - Check the three "Contents" links work
        - Click through to "Report Wreck Material"
    - Click "Start now"
      - When asked, use the email `<firstname>.<lastname>+<YYYY-MM-DD>@<domain>.<tld>` which, for Gmail at least, will be sent to `<firstname>.<lastname>@<domain>.<tld>`
      - Check you can make it all the way through to the point of submission (It will say "Accept and send" under the "Declaration by finder"
      - **Dev & Staging Only**
        - Click "Accept and send" to send the report
        - On the confirmation page check that the "Print a copy of your report" button works
        - Check that the "Check report status" button takes you to the "Check the status of wreck material you have reported" page that includes the "Sign in" button
  - Return to the home page
    - Staging * Dev
      - Click the "Check the status of wreck material reported" link
      - Check you are on the "Check the status of wreck material you have reported"
        - Check the "submitting a report" link return you to the "Report Wreck Material" page
        - Use the browser's back button to return to the "Check the status of wreck material you have reported"
      - Click on the "create an account" link and check you are on the "Sign up" page
        - **Dev & Staging Only** (keeping this because we want to get the environments aligned in future)
          - Create an account
          - Use the email you used to report the wreck
          - Once registered
            - Check that you are on the "Your reports of wreck material" page
            - Check that you can view your report
            - Click "Back to reports"
            - Click "Logout"
          - Check you are on the "Check the status of wreck material you have reported" page
            - Check you can sign in again
            - Logout
            - Check you can reset your password
              - After resetting it should redirect to the "Check the status of wreck material you have reported" page
              - Logout
    - Production
      - Click the "After you've reported wreck material" link
      - Click on the "Start now" link
        - Check you are on the "Receiver of Wreck account sign up" page
        - Check it loads as expected
        - Use browser back button to return to the "After you've reported wreck material" page
      - Click the "Sign in to your account" link
        - Check you are on the "Receiver of Wreck account sign in" page
        - Check it loads as expected
        - Click the "Forgot your password?" link
        - Check you are on the "Receiver of Wreck account password reset" page
        - Check it loads as expected
- **Backoffice**
  - Check the healthcheck endpoint - It should say "Healthy"
  - Log in using your "...@mcga.onmicrosoft.com" account
    - Check you are on the "My Dashboard" page
    - Check it shows panels for "My Assigned Droits" and "QC Approved Letters"
    - Click the navigation "Dashboard" link to check it loads the "My Dashboard" page
  - Click on the navigation "Droits" link
    - Check you are on the "Droit Reports" page
    - Click on "View" for a Droit
      - Sanity check that all the tabs load OK
      - Click "View Droits" to return to the "Droit Reports" page
    - Click "Edit" for a Droit
      - Sanity check that all the tabs load OK in edit mode
      - Click "Cancel" to return to the "Droit Reports" page
  - Click on the navigation "Wrecks" link
    - Check you are on the "Wrecks" page
    - Click on "View" for a Wreck
      - Sanity check that all the tabs load OK
      - Click "View Wrecks" to return to the "Wrecks" page
    - Click "Edit" for a Wreck
      - Sanity check that all the tabs load OK in edit mode
      - Click "Cancel" to return to the "Wrecks" page
  - Click on the navigation "Letters" link
    - Click on "View" for a Letter
      - Sanity check that all the tabs load OK
  - Click on the navigation "Salvors" link
    - Check you are on the "Salvors" page
    - Click on "View" for a Salvors
      - Sanity check that all the tabs load OK
      - Click "View Salvors" to return to the "Salvors" page
    - Click "Edit" for a Salvors
      - Sanity check that all the tabs load OK in edit mode
      - Click "Cancel" to return to the "Salvors" page

## Secrets

Secrets are set in the Repository and injected during deployment.

## License

Unless stated otherwise, the codebase is released under [the MIT License][mit]. This covers both the codebase and any
sample code in the documentation.

The documentation is [&copy; Crown copyright][copyright] and available under the terms of the [Open Government 3.0][ogl]
licence.

[mit]: LICENCE
[copyright]: http://www.nationalarchives.gov.uk/information-management/re-using-public-sector-information/uk-government-licensing-framework/crown-copyright/
[ogl]: http://www.nationalarchives.gov.uk/doc/open-government-licence/version/3/
