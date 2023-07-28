
# DROITS Infrastrucure as Code

## What is Infrastructure as Code?

- IaC is a way of setting up infrastructure resources (like virtual machines, networks and databases), via a programming language or automated tool.
  Terraform is one such programming language used for this purpose, but there are others too: for example, Ansible and even [Python](https://www.freecodecamp.org/news/what-is-infrastructure-as-code/)!
- The reason why we're using this automated approach is to make our infrastructure easily repeatable and more robust (since we can remove a lot of human error). We can even run automated tests on Terraform code before we run it.
- [Terraform beginner&#39;s guide](https://developer.hashicorp.com/terraform/intro)

## Summary of the DROITS Infrastructure

- DROITS has a VPC (virtual private cloud) which houses
  - 2 public subnets: one in each of two availability zones
  - 2 private subnets: one in each of two availability zones
  - These were provisioned manually
- S3 bucket to hold the Terraform statefile (this is how Terraform keeps track of what infrastructure it has and hasn't created in the given environment). This was provisioned manually and then linked with Terraform in the root main.tf.
- Other S3 buckets for wreck images and load balancer logs in ./s3
- Postgres DB provisioned in ./rds/
- ECS cluster, container services, and task definitions in ./ecs
  - Each container service is configured to operate behind a load balancer
- IAM role to execute ECS tasks in ./iam/
- Load balancers for each ECS service in ./alb/
- Security groups which set out rules for what kind of network traffic is allowed to and from the ECS services, DB, and load balancers in ./security-groups
- CloudWatch alarms, log groups, and dashboard for us to monitor our applications in ./cloudwatch/
- SNS subscriptions and topics to notify a sepcific email address when a CloudWatch alarm triggers in ./sns/
- [DROITS architecture Miro board](https://miro.com/app/board/uXjVPXCgex4=/)

## File Structure

- We've structured the code in a way that leverages reuseable modules of Terraform code. E.g:
  - The SNS topics and subscriptions must be provisioned for the backoffice ECS service, webapp ECS service, DB, backoffice load balancer and webapp load balancer.
  - We are reusing a single SNS module multiple times, passing in different input variables to configure the same infrastructure for each of the different resources.
  - Modules can be found in their named directories: e.g the SNS module code lives in ./sns/
- We have one root main.tf file that sets up the connection to the given AWS environment and invokes each module
- We are using the workspaces feature of Terraform which allows us to name the specific environment we are deploying to
- We are using .tfvars files at the root level to pass in environment-specific variables. E.g: dev.tfvars contains dev-specific values for all the input variables going into the root main.tf file.
- We are using TF_VAR environment variables for secret/sensitive variables. These are marked sensitive=true in the root variables.tf file and are configured in GitHub Secrets.
- In each pipeline, we export these in Bash using the syntax `export TF_VAR_alert_email_address=[value]`

## Prerequisites

- Terraform v1.0.2 or higher

## How to Locally Develop DROITS Infrastructure

- Install [Terraform](https://developer.hashicorp.com/terraform/downloads)
- Connect to the AWS DROITS Development environment

  - Install the AWS CLI
  - Open the credentials file in the .aws directory on your machine
  - Navigate to DROITS - Development > 'Command line or programmatic access' in the MCA AWS portal
  - Add the DROITS - Development profile to your .aws/credentials file using the profile name [droits_dev].
  - Your newly updated credentials file will look like so:

```
[droits_dev]
aws_access_key_id=[value]
aws_secret_access_key=[value]
aws_session_token=[value]
```

- These dev credentials are set to expire once every 8 hours. You can use this profile to connect to AWS using the CLI and using the AWS extension in VS Code.
- Toubleshooting: if you get authentication/access denied errors, you may need to set the dev credentials as environment variables in Bash. To do this, create a .sh file and write the following to it, using the values in the 'Command line or programmatic access' section in the DROITS - Development AWS:

```
export AWS_PROFILE=droits_dev
export AWS_ACCESS_KEY_ID=[value]
export AWS_SECRET_ACCESS_KEY=[value]
export AWS_SESSION_TOKEN=[value]  
```

- Then run this file from the ./terraform directory: `source [file_name].sh`
- Terraform/the AWS CLI looks at your Bash variables first, and then next in the priority order is your credentials file.
- Use either the VS Code extension or the AWS CLI to check you are properly authenticated:

  - VS Code: in the left hand pane, you should see the resources currently provisioned in the dev environment. E.g there should be an S3 bucket called droits-wreck-images
  - AWS CLI: run a command such as ``aws s3 ls`` which should list all the S3 buckets in the dev environment
- Set up the sensitive environment variables locally

  - You will need to set the sensitive variables as TF_VARs. To do this, make a new .sh file and write the following to it:

```
export TF_VAR_ecr_repository_url=[value]
export TF_VAR_api_backoffice_ecr_repository_name=[value]
export TF_VAR_webapp_ecr_repository_name=[value]
export TF_VAR_backoffice_image_tag=[value]
export TF_VAR_aws_account_number=[value]
export TF_VAR_db_username=[value]
export TF_VAR_db_password=*[value]
export TF_VAR_alert_email_address=[value]
export TF_VAR_image_tag=[value]
```

- The actual sensitive values you will use in place of [value] can be found in the 'Dev env Terraform variables script' entry in the mca-droits 1Password vault
- Run ``terraform init``
- List the current workspaces available
  ``terraform workspace list``
- Select the dev workspace
  ``terraform workspace select dev``
- Add your Terraform code!
- Run ``terraform validate``
- Run ``terraform plan -input=false -var-file=dev.tfvars`` to see a plan of the new resources your code will create, telling Terraform to look at the dev.tfvars file for all the values it needs for the root main.tf file's input variables
- If it all looks sensible, run `` terraform apply -input=false -var-file=dev.tfvars``
- Before committing your changes to git, run ``terraform fmt --recursive``

## Open Source Modules Used

- In addition to our own Terraform code, we've used some popular open source modules recorded in the Terraform Registry to speed up our development of the CloudWatch alarms.
- [Cloudposse container service alarms](https://registry.terraform.io/modules/cloudposse/ecs-cloudwatch-sns-alarms/aws/latest)
- [lorenzoaiello load balancer alarms](https://registry.terraform.io/modules/lorenzoaiello/alb-alarms/aws/latest)
- [lorenzoaiello database alarms](https://registry.terraform.io/modules/lorenzoaiello/rds-alarms/aws/latest)
