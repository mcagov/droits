# DROITS CI/CD Workflows

Step 1 : Terraform lint (checks terraform before starting)
Stage 2: Trigger webapp & backoffice pipelines concurrently :
Webapp pipeline:
Test - Currently only does a print out,
Publish -
Authenticates with AWS
Builds Docker image
Push Docker image to ECR (with tag:latest)
Backoffice pipeline:
Test - Builds application and runs unit tests
Publish -
Authenticates with AWS
Builds Docker image
Push Docker image to ECR (with tag:latest)
Stage 3: Once both pipelines have run successfully, deploys terraform changes (which releases the latest images)