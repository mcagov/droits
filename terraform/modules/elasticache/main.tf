resource "aws_elasticache_subnet_group" "main" {
  name       = "${var.application_name}-elasticache-subnet-group"
  subnet_ids = var.public_subnets
}

resource "aws_elasticache_cluster" "main" {
  cluster_id           = "${terraform.workspace}-mca-beacons-elasticache-cluster"
  engine               = "redis"
  node_type            = "cache.t2.micro"
  num_cache_nodes      = 1
  parameter_group_name = "default.redis6.x"
  engine_version       = "6.x"
  port                 = var.redis_port
  subnet_group_name    = aws_elasticache_subnet_group.main.name
  security_groups      = var.security_groups
}