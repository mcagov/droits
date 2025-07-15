##### DROITS

 DROITS is a web application that enables the Marine and Coastguard Agency to manage sea wreckage reported by members of the public. It has two groups of users:

- members of the public who find items of wreckage. They use the main front end UI contained in this repo.
- receivers of wreck. They use a Microsoft back office component to the system not contained in this repo.

## Quick Start 🚀

 The application shows a form that members of the public fill out and submit in order to report wreckages.

#### Installs dependencies

# Ubiquitous Language

- Wreck: a piece of sea wreckage washed up on land. In the code, this is referred to as 'property'
- Droit: a report of a piece of sea wreckage submitted by the public using this app
- Receivers of wreck: Marine and Coastguard Agency employees who receive the reports of wreckage submitted via this app and decide what to do with it

# Tools and Technologies Used

- Node.js
- Express.js
- HTML
- Nunjucks
- Sass
- Microsoft Power Automate Flows
- Microsoft Power Apps

# Architecture

 The system uses the following components:

- A public-facing front end web app written in HTML, Nunjucks and Sass
- A Node + Express API that serves two purposes:
  - it acts as a router, rendering specific HTML views for the different steps in the form
  - it sends the public user's report of a piece of wreckage to Microsoft Power Automate Flows in a POST request.
- This sets off a sequence of automated tasks in MS Power Automate Flow, which allows the receivers of wreck to examine the public user's report.

# Infrastructure

- The system uses an Azure app service (web app) served by an app service plan in the report-wreck-material-prod-rg resource group in the MCA production tenant.
- In UAT, the setup is the same, but the resources inhabit the Report-wreck-material-RG resource group in the same tenant.
- The production Power Automate Flow environment is called Report-Wreck-Material: the cloud flow that this application triggers is Wreck-Report-Submission.
- In UAT, the environment is called Report-Wreck-Material-UAT and its equivalent cloud flow is also called Wreck-Report-Submission.
- Both Power Automate Flow environments reside in the same MCA production tenant.

# Known Issues

- Inconsistent domain language (wreck /= property)
- The name 'DROITS' has no direct significance relating to the application itself

# How to run the app 🚀

## Install prerequisites

We recommend using [asdf](https://asdf-vm.com/guide/getting-started.html) to install the required tools specified in [.tool-versions](../.tool-versions).

## Install dependencies

```shell
# From the webapp directory...

 npm install
```

## Create a `.env.json` file

Populate it with the contents of the "Webapp .env.dev.json" in 1Password.

## Run the app

### Start the Express server

```shell
# From the webapp directory...

 npm run start
```

### Start both the Express server and frontend

```shell
# From the webapp directory...

 npm run dev
```

### Build static assets for production

```shell
# From the webapp directory...

 npm run build
```

## Testing

### Run the unit tests

```shell
# From the webapp directory...

npm run test
```

### Mutation testing

We use [Stryker Mutator](https://stryker-mutator.io/docs/stryker-js/introduction/) as a tool to help us understand how much we can trust our unit tests.

Every mutation that survives is a line of code that we can change without it being picked up by our unit tests.

To run the mutation tests:

```shell
# From the webapp directory...

stryker run
```

This will take a while, so you are not going to be running it after every commit.

Once it completes, there should be an HTML report in `reports/mutation/mutation.html`.

## Access the back office component

- [Log in to Microsoft Power Automate Flow](https://unitedkingdom.flow.microsoft.com/manage/environments/93b4f1ed-cbc0-4b5a-b71c-8465c4d011b7/flows/shared)
