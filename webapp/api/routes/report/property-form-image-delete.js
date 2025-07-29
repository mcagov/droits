import fs from 'fs';

import rateLimit from 'express-rate-limit';

// Rate limiter: max 10 requests per minute per IP for this route
const propertyFormImageDeleteLimiter = rateLimit({
  windowMs: 60 * 1000,
  max: 10, // limit each IP to 10 requests per windowMs
  message: { error: "Too many requests, please try again later." }
});
export default function (app) {
  app.post('/report/property-form-image-delete/:prop_id', propertyFormImageDeleteLimiter, function (req, res) {
    const id = req.params.prop_id;
    const image = req.session.data.property[id].image;

    fs.unlink(`uploads/${image}`, (err) => {
      if (err) console.log(err);
      else {
        console.log(`\nDeleted file @ uploads/${image}`);
      }
    });

    const forbiddenKeys = ['__proto__', 'constructor', 'prototype'];
    if (forbiddenKeys.includes(id)) {
      return res.sendStatus(403);
    }
    req.session.data.property[id].image = '';
    req.session.save();
    res.json();
  });
}
