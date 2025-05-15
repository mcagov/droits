# DROITS CI/CD Workflows

Each environment has its own workflow dispatch trigger:

- Dev pipeline deploys on a push into main
- Staging on a release set to "pre-release" (see handbook for further details)
- Production on a release set to "release"

Each pipeline generates fresh docker images and pushes them to env/account specific ECR repository

The environments set their own input variables (telling the shared pipeline workflow what environment to use) and
inject the secrets from their own environment in github secrets (under Settings/Environment in the repository)

Step 1 : Terraform lint (checks terraform before starting)
Stage 2: Trigger webapp & backoffice pipelines concurrently :
Webapp pipeline:
Test - Currently only does a print out,
Publish -
Authenticates with AWS
Builds Docker image
Push Docker image to ECR (with tag:[unique hash of the most recent git commit])
Backoffice pipeline:
Test - Builds application and runs unit tests
Publish -
Authenticates with AWS
Builds Docker image
Push Docker image to ECR (with tag:${github.sha})
Stage 3: Once both pipelines have run successfully, deploys terraform changes (which releases the commit's built image)
