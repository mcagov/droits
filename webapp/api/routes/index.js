import { Router } from 'express';
import removedPropertyCheck from './report/removed-property-check';
import location from './report/location';
import findDate from './report/find-date';
import vesselInformation from './report/vessel-information';
import salvagedFrom from './report/salvaged-from';
import depth from './report/depth';
import personal from './report/personal';
import vesselDescription from './report/vessel-description';
import knownWreck from './report/known-wreck';
import propertySummary from './report/property-summary';
import propertyBulk from './report/property-bulk';
import propertyForm from './report/property-form';
import propertyFormImage from './report/property-form-image';
import propertyFormImageUpload from './report/property-form-image-upload';
import propertyFormImageDelete from './report/property-form-image-delete';
import propertyFormAddress from './report/property-form-address';
import salvageAward from './report/salvage-award';
import checkYourAnswers from './report/check-your-answers';
import portalStart from './portal/start';
import portalLogin from './portal/login';
import portalLogout from './portal/logout';
import portalLoginRedirectUrl from './portal/loginRedirectUrl';
import portalDashboard from './portal/dashboard';
import portalViewReport from './portal/view-report';
import error from './portal/error';
import accountError from './portal/error';

import sendSample from './report/send-sample';
import health from './health';


export default () => {
  const app = Router();

  removedPropertyCheck(app);
  location(app);
  findDate(app);
  personal(app);
  knownWreck(app);
  vesselInformation(app);
  salvagedFrom(app);
  depth(app);
  vesselDescription(app);
  propertySummary(app);
  propertyBulk(app);
  propertyForm(app);
  propertyFormImage(app);
  propertyFormImageUpload(app);
  propertyFormImageDelete(app);
  propertyFormAddress(app);
  salvageAward(app);
  checkYourAnswers(app);

  portalStart(app);
  portalLogin(app);
  portalLogout(app);
  portalLoginRedirectUrl(app);
  portalDashboard(app);
  portalViewReport(app);
  error(app);
  accountError(app);

  sendSample(app);

  health(app);
  return app;
};
