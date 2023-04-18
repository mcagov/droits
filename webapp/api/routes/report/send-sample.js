import axios from 'axios';
const dotenv = require('dotenv');
dotenv.config();

const sampleReport = {
    "report-date": "2023-03-20",
    "wreck-find-date": "2022-01-01",
    "latitude": 51.45399,
    "longitude": -3.17463,
    "location-radius": 492,
    "location-description": "No additional info",
    "vessel-name": "",
    "vessel-construction-year": "",
    "vessel-sunk-year": "",
    "vessel-depth": null,
    "removed-from": "afloat",
    "wreck-description": "",
    "claim-salvage": "no",
    "salvage-services": "",
    "personal": {
      "full-name": "Test Salvor",
      "email": "test.salvor@madetech.com",
      "telephone-number": "07791351955",
      "address-line-1": "19 Test Close",
      "address-line-2": "Testing",
      "address-town": "Testington",
      "address-county": "South Testington",
      "address-postcode": "TE571NG"
    },
    "wreck-materials": [
      {
        "description": "empty bag",
        "quantity": "1",
        "value": ".10",
        "value-known": "yes",
        "image": "https://upload.wikimedia.org/wikipedia/commons/thumb/1/11/Test-Logo.svg/783px-Test-Logo.svg.png",
        "originalFilename": "test.png",
        "address-details": {
            "address-line-1": "19 Test Close",
            "address-line-2": "Testing",
            "address-town": "Testington",
            "address-county": "South Testington",
            "address-postcode": "TE571NG"
        },
        "storage-address": "personal"
      }
    ]
  }



export default function (app) {

    app.get('/report/send-sample', async function (req, res, next) {
        await axios.post(
            process.env.API_POST_ENDPOINT,
            sampleReport,
            {
              headers: { 'content-type': 'application/json' },
            }
          ).then((response) => {
            return res.render('report/send-sample',{data: JSON.stringify(response.data)});
          }, (error) => {
            console.log(error);
          });
    });
}
