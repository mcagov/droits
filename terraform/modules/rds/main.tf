resource "aws_db_instance" "droits" {
  allocated_storage               = var.db_allocated_storage
  db_name                         = var.db_name
  identifier                      = "${terraform.workspace}-droits-db"
  engine                          = "postgres"
  engine_version                  = "14"
  instance_class                  = "db.t3.micro"
#  username                        = var.db_username
#  password                        = var.db_password
#  db_subnet_group_name            = aws_db_subnet_group.db.id
#  vpc_security_group_ids          = [aws_security_group.db.id]
#  deletion_protection             = var.db_delete_protection
  parameter_group_name            = "default.postgres14"
  skip_final_snapshot             = true
  storage_encrypted               = true
}