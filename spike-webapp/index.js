var express = require('express');
const axios = require('axios');
var app = express();

const PORT = 3000;
const backofficeLoadBalancerUrl = "droits-api-backoffice-external-931641116.eu-west-2.elb.amazonaws.com";
const backofficeHealthUrl = "/home/droits";

app.get('/', function(req, res){
    res.send("DROITS Webapp");
});

app.get('/backoffice', function(req, res){
    axios.get(backofficeHealthUrl)
    .then((response) => {
        console.log(response);
        res.send("Success - Check logs for response.");
      }, (error) => {
        console.log(error);
        res.send(error);
      });
 });

 app.listen(PORT, () => {
    console.log(`App listening on ${PORT} - url: http://localhost:${PORT}`);
    console.log('Press Ctrl+C to quit.');
  });

