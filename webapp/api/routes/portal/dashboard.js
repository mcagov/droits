import axios from 'axios';
import dayjs from 'dayjs';
import CustomParseFormat from "dayjs/plugin/customParseFormat";

dayjs.extend(CustomParseFormat);
import ensureAuthenticated from './ensureAuthenticated';
import {assignSalvorInfoReportStatus} from "../../../utilities/assignReportStatus";

require("dotenv-json")();

const url = `${process.env.API_ENDPOINT}/api/salvor`
export default function (app) {
  app
    .get('/portal/dashboard', ensureAuthenticated, function (req, res) {
      const currentUserEmail = req.user.emails[0] || req.session.user.emails[0];

      let userReports = [];

      fetchSalvorInfo(url, currentUserEmail, userReports, res, req)
      .then(() => {
        return res.render('portal/dashboard', { userReports: userReports });
      })
    });


const fetchSalvorInfo = (url, currentUserEmail, userReports, res, req) => {
  url = `${url}?email=${encodeURIComponent(currentUserEmail)}`;
  return new Promise((resolve, reject) => {
    axios
        .get(url, {
            headers: {
              'X-API-Key': process.env.API_KEY
            }
        })
        .then((res) => {
          
          const reportData = res.data.reports;
          const session = req.session.data;
          session.userId = res.data.id || "";
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

