resource "aws_db_subnet_group" "droits_db" {
  name       = "${terraform.workspace}-droits-db-subnet-group"

  subnet_ids = [
    var.public_subnet_1,
    var.public_subnet_2
  ]

  tags = {
    Name = "DROITS DB subnet group"
  }
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
  db_subnet_group_name            = aws_db_subnet_group.droits_db.name
  vpc_security_group_ids          = [var.db_security_group_id]
  deletion_protection             = var.db_delete_protection
  parameter_group_name            = "default.postgres14"
  skip_final_snapshot             = true
  storage_encrypted               = true
}