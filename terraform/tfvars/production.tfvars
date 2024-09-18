ecs_cluster_name          = "droits-cluster"
webapp_fargate_cpu        = 1024
webapp_fargate_memory     = 2048
backoffice_fargate_cpu    = 2048
backoffice_fargate_memory = 6144

db_name              = "droits-db"
db_allocated_storage = 150
db_delete_protection = true
db_instance_class    = "db.t3.large"
db_storage_encrypted = true

aws_region = "eu-west-2"

root_domain_name = "report-wreck-material.service.gov.uk"

a_records = [
  {
    name        = "report-wreck-material.service.gov.uk"
    application = "webapp"
  },
  {
    name        = "www.report-wreck-material.service.gov.uk"
    application = "webapp"
  },
  {
    name        = "webapp.report-wreck-material.service.gov.uk"
    application = "webapp"
  },
  {
    name        = "backoffice.report-wreck-material.service.gov.uk"
    application = "backoffice"
  }
]

ssl_domains = [
  "report-wreck-material.service.gov.uk",
  "backoffice.report-wreck-material.service.gov.uk"
]

delegated_hosted_zones = {
  "dev.report-wreck-material.service.gov.uk" : [
    "ns-2029.awsdns-61.co.uk.",
    "ns-592.awsdns-10.net.",
    "ns-156.awsdns-19.com.",
    "ns-1326.awsdns-37.org."
  ],
  "dev.backoffice.report-wreck-material.service.gov.uk" : [
    "ns-2029.awsdns-61.co.uk.",
    "ns-592.awsdns-10.net.",
    "ns-156.awsdns-19.com.",
    "ns-1326.awsdns-37.org."
  ],
  "staging.report-wreck-material.service.gov.uk" : [
    "ns-1035.awsdns-01.org.",
    "ns-1889.awsdns-44.co.uk.",
    "ns-980.awsdns-58.net.",
    "ns-427.awsdns-53.com."
  ],
  "staging.backoffice.report-wreck-material.service.gov.uk" : [
    "ns-1035.awsdns-01.org.",
    "ns-1889.awsdns-44.co.uk.",
    "ns-980.awsdns-58.net.",
    "ns-427.awsdns-53.com."
  ]
}

lb_ssl_policy               = "ELBSecurityPolicy-FS-1-2-2019-08"

enable_alerts                                      = false
percentage_cpu_utilization_high_threshold          = 90
percentage_memory_utilization_high_threshold       = 90
cpu_utilisation_duration_in_seconds_to_evaluate    = 300
cpu_utilization_high_evaluation_periods            = 1
memory_utilisation_duration_in_seconds_to_evaluate = 300
memory_utilization_high_evaluation_periods         = 1

db_evaluation_periods                              = "1"
db_cpu_credit_balance_too_low_threshold            = "100"
db_maximum_used_transaction_ids_too_high_threshold = "1000000000"
db_memory_freeable_too_low_threshold               = "256000000"
db_memory_swap_usage_too_high_threshold            = "256000000"

lb_average_response_time_threshold = "600"
lb_evaluation_periods              = "1"
