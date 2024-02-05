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
      const currentUserEmail = req.user.emails[0];
      let userReports = [];

        fetchSalvorInfo(url, currentUserEmail, userReports, res).then(
          () => {
            return res.render('portal/dashboard', { userReports: userReports });
          }
        );
      // }).catch(() => {
      //   req.logOut();
      //   return res.redirect('/account-error');
      });
          
          
          console.log("Sending Request");

const fetchSalvorInfo = (url, currentUserEmail, userReports, res) => {
  url = `${url}?email=${encodeURIComponent(currentUserEmail)}`;
  return new Promise((resolve, reject) => {
    axios
        .get(url)
        .then((res) => {
          const reportData = res.data.reports;
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
          }
          reject();
        });
  });
};
   
      // function getUserData(token) {
      //   return new Promise((resolve, reject) => {
      //     axios
      //       .get(encodedContactsUrl, {
      //         headers: { Authorization: `bearer ${token}` },
      //       })
      //       .then((res) => {
      //         const data = res.data.value[0];
      //         const session = req.session.data;
      //         // currentUserID = data.contactid;
      //         session.id = currentUserID;
      //         session.userName = data.fullname;
      //         session.userEmail = data.emailaddress1;
      //         session.userTel = data.telephone1;
      //         session.userAddress1 = data.address1_line1;
      //         session.userAddress2 = data.address1_line2;
      //         session.userCity = data.address1_city;
      //         session.userCounty = data.address1_county;
      //         session.userPostcode = data.address1_postalcode;
      //         resolve();
      //       })
      //       .catch((reqError) => {
      //         console.log('User ID error');
      //         console.log(reqError);
      //         reject();
      //       });
      //   });
      // }
    // })

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

