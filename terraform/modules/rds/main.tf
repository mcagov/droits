resource "aws_db_subnet_group" "droits_db" {
  name = "${terraform.workspace}-droits-db-subnet-group"

  subnet_ids = module.vpc.public_subnets

  tags = {
    Name = "DROITS DB subnet group"
  }
}

resource "aws_db_instance" "droits" {
  allocated_storage      = var.db_allocated_storage
  db_name                = var.db_name
  identifier             = "${terraform.workspace}-droits-db"
  engine                 = "postgres"
  engine_version         = "14"
  instance_class         = var.db_instance_class
  username               = var.db_username
  password               = var.db_password
  db_subnet_group_name   = aws_db_subnet_group.droits_db.name
  vpc_security_group_ids = [module.security-groups.db-security-group-id]
  deletion_protection    = var.db_delete_protection
  parameter_group_name   = "default.postgres14"
  skip_final_snapshot    = true
  storage_encrypted      = true
}
