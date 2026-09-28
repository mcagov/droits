#!/bin/sh
set -eu

awslocal s3 mb "s3://${BUCKET_NAME}"
