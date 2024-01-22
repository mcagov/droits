#!/bin/bash


source get_access_token.sh

REQUEST_ENDPOINT="crf99_mcawreckses?\$top=10&\$select=crf99_mcawrecksid,crf99_name,crf99_wrecktype,crf99_longitude,crf99_latitude,crf99_iswarwreck,crf99_isaircraft,createdon,crf99_dateofloss,crf99_protectedsite,_crf99_protectionlegislation_value&\$expand=crf99_WreckOwner(\$select=fullname,emailaddress1,address1_line1,address1_city,address1_postalcode,address1_composite,mobilephone,createdon)"

sh powerapps_api.sh $access_token "output.json" $REQUEST_ENDPOINT
