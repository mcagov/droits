import axios from "axios";
import {assignSalvorInfoReportStatus} from "../../../utilities/assignReportStatus";
import dayjs from "dayjs";

const apiEndpoint = "http://localhost:5000";
export default function (app) {
  app.get('/portal/droit/:droitId', async function (req, res) {
    try {
      const droitId = req.params.droitId;
      const apiUrl = `${apiEndpoint}/api/droit/${droitId}`;

      const response = await axios.get(apiUrl);
      const reportData = formatReportData(response.data);

      res.render("portal/droit",{ reportData: reportData });
    } catch (error) {
      console.error('Error:', error);
      res.status(500).send('Internal Server Error');
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

  reportItem['base_image_url'] = `${apiEndpoint}/Image/DisplayImage`
  return reportItem;
};
