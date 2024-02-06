import axios from "axios";
import {assignSalvorInfoReportStatus} from "../../../utilities/assignReportStatus";
import dayjs from "dayjs";

export default function (app) {
  app.get('/portal/report/:droitId', async function (req, res) {
    try {
      const session = req.session.data;

      const droitId = req.params.droitId;
      const salvorId = session.userId;
      const apiUrl = `${process.env.API_ENDPOINT}/api/droit/${droitId}?salvorId=${salvorId}`;


      const response = await axios.get(apiUrl, {
        headers: {
          'X-API-Key': process.env.API_KEY
        }
      });
      const reportData = formatReportData(response.data);

      res.render("portal/report",{ reportData: reportData });
    } catch (error) {
      console.error('Error:', error.message);
      res.render('portal/unauthorized', {error: error.message} )
    }
  });
}

export const formatReportData = (data) => {
  const statusCode = data.status;

  let reportItem = data;

  reportItem['date-found'] = dayjs(data.date_found, 'DD/MM/YYYY HH:mm:ss' ).format('DD MMM YYYY');
  reportItem['date-reported'] = dayjs(data.date_reported, 'DD/MM/YYYY HH:mm:ss' ).format(
      'DD MMM YYYY'
  );
  reportItem['last-updated'] = dayjs(data.last_updated,  'DD/MM/YYYY HH:mm:ss' ).format('DD MMM YYYY');

  const reportStatus = assignSalvorInfoReportStatus(statusCode);

  reportItem['status'] = reportStatus[0];
  reportItem['status-attr'] = reportStatus[1];
  reportItem['status-colour'] = reportStatus[2];

  reportItem['base_image_url'] = `${process.env.API_ENDPOINT}/Image/DisplayImage`
  return reportItem;
};
