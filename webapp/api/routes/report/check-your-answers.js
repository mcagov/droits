import axios from 'axios';
import fs from 'fs';
import path from 'path';
import {allWmContainImages, formatValidationErrors} from '../../../utilities';

const { body, validationResult } = require('express-validator');
var cloneDeep = require('lodash.clonedeep');
export default function (app) {
  // If user clicks back button on confirmation page it will redirect to start page
  app.get('/report/check-your-answers', function (req, res, next) {
    if (Object.keys(req.session.data['report-date']).length === 0) {
      return res.redirect('/report/start');
    }
    next();
  });

  app.post(
    '/report/confirmation',
    [
      body('property-declaration')
        .exists()
        .not()
        .isEmpty()
        .withMessage('Select to confirm you are happy with the declaration'),
      body('change-wm-link')
          .custom((value, {req}) => {
            return allWmContainImages(req.session.data['property']);
          })
          .withMessage('An image is required for each wreck material')
    ],
    async function (req, res) {
      const errors = formatValidationErrors(validationResult(req));
      const sd = cloneDeep(req.session.data);

      // Errors
      if (errors) {
        return res.render('report/check-your-answers', {
          errors,
          errorSummary: Object.values(errors),
          values: req.body,
        });
      } else {
        // Check if values exist in session
        let textLocation = sd['location']['text-location'].trimEnd();
        let locationDescription = sd['location']['location-description'];
        let formattedTextLocation;
        // If user has chosen to provide the location in text form rather than coordinates,
        // format the text so it can be concatenated with any (optional) additional details/description text
        if (textLocation.length) {
          formattedTextLocation = textLocation.endsWith('.')
            ? textLocation
            : textLocation + '.';
        }
        let concatenatedText =
          formattedTextLocation + ' ' + locationDescription;
        const locationDetails =
          formattedTextLocation !== undefined
            ? concatenatedText
            : locationDescription;

        console.dir(req.session.data);
        console.dir(sd);
        // Data obj to send to db
        const data = {
          'report-date': `${sd['report-date']['year']}-${sd['report-date']['month']}-${sd['report-date']['day']}`,
          'wreck-find-date': `${sd['wreck-find-date']['year']}-${sd[
            'wreck-find-date'
          ]['month'].padStart(2, '0')}-${sd['wreck-find-date']['day'].padStart(
            2,
            '0'
          )}`,
          latitude: sd['location']['location-standard']['latitude'],
          longitude: sd['location']['location-standard']['longitude'],
          'location-radius': sd['location']['location-standard']['radius'],
          'location-description': locationDetails,
          'vessel-name': sd['vessel-information']['vessel-name'],
          'vessel-construction-year':
            sd['vessel-information']['vessel-construction-year'],
          'vessel-sunk-year': sd['vessel-information']['vessel-sunk-year'],
          'vessel-depth': sd['vessel-depth'],
          'removed-from': sd['removed-from'],
          'wreck-description': sd['wreck-description'],
          'claim-salvage': sd['claim-salvage'],
          'salvage-services': sd['salvage-services'],
          personal: {
            'full-name': sd['personal']['full-name'],
            email: sd['personal']['email'],
            'telephone-number': sd['personal']['telephone-number'],
            'address-line-1': sd['personal']['address-line-1'],
            'address-line-2': sd['personal']['address-line-2'],
            'address-town': sd['personal']['address-town'],
            'address-county': sd['personal']['address-county'],
            'address-postcode': sd['personal']['address-postcode'],
          },
          'wreck-materials': [],
        };

        for (const prop in sd['property']) {
          if (sd['property'].hasOwnProperty(prop)) {
            const innerObj = sd['property'][prop];
            const filePath = path.resolve(
              __dirname,
              '../../../uploads/',
              innerObj.image
            );

            try {
              const imageData = await fs.promises.readFile(filePath, 'base64');

              innerObj.image = {
                filename: innerObj.originalFilename,
                data: imageData,
              };

              if (innerObj.value === '') {
                innerObj.value = null;
              }

              data['wreck-materials'].push(innerObj);
            } catch (error) {
              console.error('Error reading file:', error);
              data['wreck-materials'].push(innerObj);
            }
          }
        }

        var wreckMaterials = data['wreck-materials'];

        const appendToOriginalSubmission = wreckMaterials && wreckMaterials.length <= 5;

        console.log(`appending to original submission: ${appendToOriginalSubmission}`);

        console.dir(wreckMaterials);

        data['wreck-materials'] = [];

        try {
          console.dir(data);
          const response = await axios.post(
            `${process.env.API_ENDPOINT}/Api/SubmitDroit`,
            data,
            {
              headers: {
                'content-type': 'application/json',
                'X-API-Key': process.env.API_KEY,
              },
              maxBodyLength: Infinity,
              timeout: 300000
            }
          );


          if (response.status === 200) {
            let reference = response.data.reference;
            let droitId = response.data.droitId;
            console.log("Report submitted - " + reference + " - " + droitId);


            for (const wreckMaterial of wreckMaterials) {
              const wreckMaterialNumber = wreckMaterials.indexOf(wreckMaterial) + 1;
              const wreckMaterialName = `${reference}-${ wreckMaterialNumber <= 9 ? String(wreckMaterialNumber).padStart(2, '0') : wreckMaterialNumber }`;
              
              wreckMaterial['droit-id'] = droitId;
              wreckMaterial['append-to-original-submission'] = appendToOriginalSubmission;
              wreckMaterial['name'] = wreckMaterialName;
              
              console.log(`Sending wm - ${wreckMaterialName}`);
              
              try {
                const wmResponse = await axios.post(
                  `${process.env.API_ENDPOINT}/Api/SubmitWreckMaterial`,
                  wreckMaterial,
                  {
                    headers: {
                      'content-type': 'application/json',
                      'X-API-Key': process.env.API_KEY,
                    },
                    maxBodyLength: Infinity,
                    timeout: 300000
                  }
                );

                if (wmResponse.status !== 200) {
                  console.error(
                    `Posting Wreck Material to API failed for droitId ${droitId}! - ${wmResponse.status}`
                  );
                }
              } catch (error) {
                console.error(`Error posting wreck material to API: ${error}`);
              }
            }

            console.log("Sending confirmation email");

            await axios.post(
              `${process.env.API_ENDPOINT}/Api/SendConfirmationEmail`,
              droitId,
              {
                headers: {
                  'content-type': 'application/json',
                  'X-API-Key': process.env.API_KEY,
                },
                maxBodyLength: Infinity,
                timeout: 300000
              }
            );

            // Clear session data
            req.session.data = {};
            // req.session.destroy();

            return res.render('report/confirmation', { reference });
          } else {
            console.error(`Posting Droit to API failed! - ${response.status}`);
            res.redirect('/error');
          }
        } catch (err) {
          console.error(err);
        }
      }
    }
  );
}
