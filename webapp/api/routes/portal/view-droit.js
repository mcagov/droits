import axios from "axios";
import {assignSalvorInfoReportStatus} from "../../../utilities/assignReportStatus";
import dayjs from "dayjs";

export default function (app) {
  app.get('/portal/droit/:droitId', async function (req, res) {
    try {
      const droitId = req.params.droitId;
      const apiUrl = `http://localhost:5000/api/droit/${droitId}`;

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
  const wreckMaterialsData = data.wreck_materials;
  const statusCode = data.status;

  let reportItem = data;

  reportItem['date-found'] = dayjs(data.date_found, 'DD/MM/YYYY HH:mm:ss' ).format('DD MMM YYYY');
  reportItem['date-reported'] = dayjs(data.date_reported, 'DD/MM/YYYY HH:mm:ss' ).format(
    'DD MMM YYYY'
  );
  reportItem['last-updated'] = dayjs(data.last_updated,  'DD/MM/YYYY HH:mm:ss' ).format('DD MMM YYYY');
  
  // reportItem['wreck-materials'] = [];
  //
  // wreckMaterialsData.forEach((item) => {
  //   reportItem['wreck-materials'].push( { description : item.description , outcome: item.outcome, storage_address: item.storage_address, quantity: "1"});
  // });

  const reportStatus = assignSalvorInfoReportStatus(statusCode);

  reportItem['status'] = reportStatus[0];
  reportItem['status-attr'] = reportStatus[1];
  reportItem['status-colour'] = reportStatus[2];

  return reportItem;
};
