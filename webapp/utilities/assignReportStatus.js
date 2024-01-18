export const assignReportStatus = function (id) {
  let status;
  let statusAttr;
  let statusColour;

  switch (id) {
    case 614880000:
      status = "received";
      statusAttr = "received";
      statusColour = "purple";
      break;
    case 614880001:
      status = "investigation ongoing";
      statusAttr = "ongoing"
      statusColour = "blue"
      break;
    case 614880002:
      status = "closed";
      statusAttr = "closed"
      statusColour = "grey";
      break;
    case 614880004:
      status = "awaiting your response";
      statusAttr = "awaiting"
      statusColour = "orange"
      break;
  }

  return [status, statusAttr, statusColour];
};

export const assignSalvorInfoReportStatus = function (id) {
  let status;
  let statusAttr;
  let statusColour;

  switch (id) {
    case "Received":
      status = "received";
      statusAttr = "received";
      statusColour = "purple";
      break;
    case "Research":
    case "InitialResearch":
    case "AcknowledgementLetterSent":
    case "ReadyForQc":
    case "QcApproved":    
    case "NegotiatingSalvageAward":
      status = "investigation ongoing";
      statusAttr = "ongoing"
      statusColour = "blue"
      break;
    case "Closed":
      status = "closed";
      statusAttr = "closed"
      statusColour = "grey";
      break;
    case "AwaitingResponse":
      status = "awaiting your response";
      statusAttr = "awaiting"
      statusColour = "orange"
      break;
  }

  return [status, statusAttr, statusColour];
};