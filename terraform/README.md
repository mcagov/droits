# DROITS Infrastrucure as Code

## What is Infrastructure as Code?
- IaC is a way of setting up infrastructure resources (like virtual machines, networks and databases), via a programming language or automated tool.
Terraform is one such programming language used for this purpose, but there are others too: for example, Ansible and even [Python](https://www.freecodecamp.org/news/what-is-infrastructure-as-code/)!
- The reason why we're using this automated approach is to make our infrastructure easily repeatable and more robust (since we can remove a lot of human error). We can even run automated tests on Terraform code before we run it.
- [Terraform beginner's guide](https://developer.hashicorp.com/terraform/intro)


## Summary of the DROITS infrastructure
- DROITS has a VPC (virtual private cloud) which houses
    - 2 public subnets: one in each of two availability zones
    - 2 private subnets: one in each of two availability zones
    - These were provisioned manually
- S3 bucket to hold the Terraform statefile (this is how Terraform keeps track of what infrastructure it has and hasn't created in the given environment). This was provisioned manually and then linked with Terraform in the root main.tf.
- Postgres DB provisioned in ./rds/
- IAM role to execute ECS tasks in ./iam/
- 
- [DROITS architecture Miro board](https://miro.com/app/board/uXjVPXCgex4=/)

## File Structure
- Explanation of structure e.g modules and workspaces

## How to Locally Develop DROITS Infrastructure
 - Terraform  lint
- How to locallydevleop Terraform

## Open Source Modules Used
- Mention and links to Lorenzo's libraries
- Cloudposse
- Note on version numbers inline

## Sensitive Environment Variables








refactoring
- move other buckets into S3 module
- configure any more configurables
- Terratest 
- Logic?
- Opportunities for reuseable module?
    - ECS
    - ALB