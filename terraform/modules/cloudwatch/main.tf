module "aws-rds-alarms" {
  source         = "lorenzoaiello/rds-alarms/aws"
  version        = "2.2.0"
  db_instance_id = var.db_instance_id
  db_instance_class = var.db_instance_class
}

module "aws-alb-alarms" {
  source           = "lorenzoaiello/alb-alarms/aws"
  version          = "1.2.0"
  load_balancer_id = var.backoffice_alb_id
  prefix           = var.backoffice_load_balancer
  target_group_id  = var.backoffice_alb_target_group_id
}