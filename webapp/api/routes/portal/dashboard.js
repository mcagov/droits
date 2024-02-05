import axios from 'axios';
import dayjs from 'dayjs';
import CustomParseFormat from "dayjs/plugin/customParseFormat";

dayjs.extend(CustomParseFormat);
import ensureAuthenticated from './ensureAuthenticated';
import { assignReportStatus } from '../../../utilities';
import {assignSalvorInfoReportStatus} from "../../../utilities/assignReportStatus";
import res from "express/lib/response";
import req from "express/lib/request";
require("dotenv-json")();

const url = "http://localhost:5000/api/salvor"

export default function (app) {
  app
    .get('/portal/dashboard', ensureAuthenticated, function (req, res) {
      const currentUserEmail = req.user.emails[0] || req.session.user.emails[0];
      console.log(currentUserEmail);

      let userReports = [];

      fetchSalvorInfo(url, currentUserEmail, userReports, res, req)
      .then(() => {
        return res.render('portal/dashboard', { userReports: userReports });
      })
      // .catch((error) => {
      //   // Handle the error here
      //   console.error('An error occurred:', error);

      //            //   req.logOut();
      //           //   return res.redirect('/account-error');

      //           return res.status(500).json({
      //             status: 500,
      //             message: 'An error occurred while fetching Salvor information. Please try again later.',
      //             error: error.message // Optionally include the error message
      //         });


      //   // Render an error page or do something else as needed
      //   // return res.render('error', { errorMessage: 'An error occurred. Please try again later.' });
      // });
      });


          console.log("Sending Request");

const fetchSalvorInfo = (url, currentUserEmail, userReports, res, req) => {
  url = `${url}?email=${encodeURIComponent(currentUserEmail)}`;
  return new Promise((resolve, reject) => {
    axios
        .get(url)
        .then((res) => {
          const reportData = res.data.reports;
          const session = req.session.data;
          session.userName = res.data.name || "";
          session.userEmail = res.data.email || "";

          session.userTel = res.data.telephoneNumber || "";
          session.userAddress1 = res.data.address.line1 || "";
          session.userAddress2 = res.data.address.line2 || "";
          session.userCity = res.data.address.city || "";
          session.userCounty = res.data.address.county || "";
          session.userPostcode = res.data.address.postcode || "";

          reportData.forEach((item) => {
            formatSalvorInfo(item, userReports);
          });
          resolve();
        })
        .catch((err) => {
          console.log('[Report data error]:' + err);
          if (err.response.status === 401) {
            req.logOut();
            res.redirect('/error');
          } else if (err.response.status === 404) {
            // Handle 404 response here
            res.render('portal/unknown-salvor', {salvorEmail: currentUserEmail} )
          } else {
            reject();
          }
        });
  });
};


    // Sorting reports
    // .post(
    //   '/portal/dashboard',
    //
    //   function (req, res) {
    //     const type = req.body['report-sort-by'];
    //     const accessToken = req.session.data.token;
    //     const filteredReportUrl = `${url}crf99_mcawreckreports?$filter=_crf99_reporter_value eq ${req.session.data.id}&$expand=crf99_MCAWreckMaterial_WreckReport_crf99_($select=crf99_description)&$orderby=${type} desc`;
    //
    //     let userReports = [];
    //
    //     fetchReportData(accessToken, filteredReportUrl, userReports, res).then(
    //       () => {
    //         return res.render('portal/dashboard', {
    //           userReports: userReports,
    //           sort: type,
    //         });
    //       }
    //     );
    //   }
    // );
}



export const formatSalvorInfo = (data, userReports) => {
  const wreckMaterialsData = data.wreck_materials;
  const statusCode = data.status;

  let reportItem = {};

  reportItem['report-id'] = data.id;
  reportItem['report-ref'] = data.reference;
  reportItem['date-found'] = dayjs(data.date_found, 'DD/MM/YYYY HH:mm:ss' ).format('DD MMM YYYY');
  reportItem['date-reported'] = dayjs(data.date_reported, 'DD/MM/YYYY HH:mm:ss' ).format(
    'DD MMM YYYY'
  );
  reportItem['last-updated'] = dayjs(data.last_updated,  'DD/MM/YYYY HH:mm:ss' ).format('DD MMM YYYY');
  reportItem['wreck-materials'] = [];

  wreckMaterialsData.forEach((item) => {
    reportItem['wreck-materials'].push(item.description);
  });

  const reportStatus = assignSalvorInfoReportStatus(statusCode);

  reportItem['status'] = reportStatus[0];
  reportItem['status-attr'] = reportStatus[1];
  reportItem['status-colour'] = reportStatus[2];

  userReports.push(reportItem);
  return userReports;
};

