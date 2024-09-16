import deepFreeze from "../../utilities/deepFreeze";

/*

Provide default values for user session data. These are automatically added
via the `sessionData` middleware. Values will only be added to the
session if a value doesn't already exist. This may be useful for testing
journeys where users are returning or logging in to an existing application.

============================================================================

Example usage:

"full-name": "Sarah Philips",

"options-chosen": [ "foo", "bar" ]

============================================================================

*/

const defaultData= {
  'report-date': {},
  'wreck-find-date': {
    day: '',
    month: '',
    year: ''
  },
  personal: {
    'full-name': '',
    email: '',
    'telephone-number': '',
    'address-line-1': '',
    'address-line-2': '',
    'address-town': '',
    'address-county': '',
    'address-postcode': ''
  },
  location: {
    'location-standard': {},
    'location-given': {},
    'text-location': '',
    'location-description': ''
  },
  property: {},
  'vessel-information': {
    'vessel-name': '',
    'vessel-construction-year': '',
    'vessel-sunk-year': ''
  },
  'vessel-depth': null,
  'wreck-description': '',
  'salvage-services': '',
  submittedFiles: []
};

// const frozenDefaultData = deepFreeze(defaultData);

// Object.freeze(defaultData["report-date"])
// Object.freeze(defaultData["wreck-find-date"])
// Object.freeze(defaultData.personal)
// Object.freeze(defaultData.location)
// Object.freeze(defaultData.location["location-standard"])
// Object.freeze(defaultData.location["location-given"])
// Object.freeze(defaultData.property)
// Object.freeze(defaultData["vessel-information"])
// Object.freeze(defaultData.submittedFiles)


 export default defaultData;