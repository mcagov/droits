# DROITS CI/CD Workflows

- Explain that each  env is in different AWS account so there's not promotion of dcker images e.g from staging to prd
- Each pipeline generates fresh docker images and pushes them to env/account specific ECR repository
- Reuse of pipeline.yml
- Explain inputs
- Explain secrets

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
