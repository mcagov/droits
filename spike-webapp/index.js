var express = require('express');
const axios = require('axios');
var app = express();

const PORT = 3000;
const backofficeLoadBalancerUrl = "http://api-backoffice-alb-1563299445.eu-west-2.elb.amazonaws.com";

app.get('/', function(req, res){
    res.send("DROITS Webapp - Tagged");
});

app.get('/health', function(req, res){
    res.send("OK");
});

app.get('/backoffice', function(req, res){
    axios.get(`${backofficeLoadBalancerUrl}`)
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

