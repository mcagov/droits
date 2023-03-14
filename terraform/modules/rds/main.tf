module "network" {
  source = "../network"
}


resource "aws_db_instance" "droits" {
  allocated_storage               = var.db_allocated_storage
  db_name                         = var.db_name
  identifier                      = "${terraform.workspace}-droits-db"
  engine                          = "postgres"
  engine_version                  = "14"
  instance_class                  = "db.t3.micro"
  username                        = local.envs["DB_USERNAME"]
  password                        = local.envs["DB_PASSWORD"]
  // define the next two as vars in rds/ variables.tf
  // avoid importing network module
  db_subnet_group_name            =  module.network.public-subnet-1
  vpc_security_group_ids          = [module.network.db-security-group-id]
  deletion_protection             = var.db_delete_protection
  parameter_group_name            = "default.postgres14"
  skip_final_snapshot             = true
  storage_encrypted               = true
}