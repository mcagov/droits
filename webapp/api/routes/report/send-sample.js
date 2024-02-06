import axios from 'axios';
require("dotenv-json")();

const sampleReport = {
    "report-date": "2023-12-01",
    "wreck-find-date": "2022-01-01",
    "latitude": 51.45399,
    "longitude": -3.17463,
    "location-radius": 492,
    "location-description": "No additional info",
    "vessel-name": "Test Vessel",
    "vessel-construction-year": "2011",
    "vessel-sunk-year": "2012",
    "vessel-depth": null,
    "removed-from": "afloat",
    "wreck-description": "Some wreck description",
    "claim-salvage": "no",
    "salvage-services": "Some salvage services",
    "personal": {
        "full-name": "Test Salvor",
        "email": "sam.kendell+test_salvor@madetech.com",
        "telephone-number": "07791351955",
        "address-line-1": "19 Test Close",
        "address-line-2": "Testing",
        "address-town": "Testington",
        "address-county": "South Testington",
        "address-postcode": "TE571NG"
    },
    "wreck-materials": [
        {
            "description": "empty bag 2",
            "quantity": "1",
            "value": ".10",
            "value-known": "yes",
            "image":{
                "filename": "sk-test.jpg",
                "data" : "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"}
            ,
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
            `${process.env.API_ENDPOINT}/api/send`,
            sampleReport,
            {
              headers: { 'content-type': 'application/json' , 'X-API-Key': 'xxx'}
            }
          ).then((response) => {
            return res.render('report/send-sample',{data: JSON.stringify(response.data)});
          }, (error) => {
            console.error(error);
            return res.status(500).send('Error sending sample report - '+error);
          });
    });
}
