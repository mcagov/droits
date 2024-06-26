resource "aws_db_subnet_group" "db_subnet_group" {
  name = "${terraform.workspace}-db-subnet-group"

  subnet_ids = var.public_subnets

  tags = {
    Name = "DROITS DB subnet group"
  }
}

resource "aws_db_instance" "droits" {
  allocated_storage       = var.db_allocated_storage
  db_name                 = var.db_name
  identifier              = "${terraform.workspace}-db"
  engine                  = "postgres"
  engine_version          = "14"
  instance_class          = var.db_instance_class
  username                = var.db_username
  password                = var.db_password
  db_subnet_group_name    = aws_db_subnet_group.db_subnet_group.name
  vpc_security_group_ids  = var.db_security_groups
  deletion_protection     = var.db_delete_protection
  parameter_group_name    = "default.postgres14"
  skip_final_snapshot     = false
  storage_encrypted       = true
  backup_retention_period = 14
  apply_immediately       = true

  lifecycle {
    create_before_destroy = true
  }
}
