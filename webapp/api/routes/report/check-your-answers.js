import axios from 'axios';
import fs from 'fs';
import path from 'path';
import {formatValidationErrors} from '../../../utilities';

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
          formattedTextLocation = textLocation.endsWith(".") ? textLocation : textLocation + ".";
        }
        let concatenatedText = formattedTextLocation + " " + locationDescription;
        const locationDetails = formattedTextLocation !== undefined ? concatenatedText : locationDescription;

        // Data obj to send to db
        const data = {
          'report-date': `${sd['report-date']['year']}-${sd['report-date']['month']}-${sd['report-date']['day']}`,
          'wreck-find-date': `${sd['wreck-find-date']['year']}-${sd['wreck-find-date']['month']}-${sd['wreck-find-date']['day']}`,
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
            const filePath = path.resolve(__dirname, '../../../../uploads/', innerObj.image);

            try {
              const imageData = await fs.promises.readFile(filePath, 'base64');
              const fileName = innerObj.image;

              innerObj.image = {
                filename: fileName,
                data: imageData
              };

              data['wreck-materials'].push(innerObj);
            } catch (error) {
              console.error('Error reading file:', error);
            }
          }
        }


        try {
          const response = await axios.post(
            process.env.API_POST_ENDPOINT,
            JSON.stringify(data),
            {
              headers: { 'content-type': 'application/json' },
            }
          );

          if (response.status === 200) {

            let reference = response.data.reference;

            // Clear session data
            req.session.data = {};

            return res.render('report/confirmation', { reference });
          }

        } catch (err) {
          console.error(err);
        }
      }
    }
  );
}
