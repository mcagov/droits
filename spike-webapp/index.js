var express = require('express');
const axios = require('axios');
var app = express();
const PORT = 3000;
const backofficeHealthUrl = "http://www.google.com"

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

