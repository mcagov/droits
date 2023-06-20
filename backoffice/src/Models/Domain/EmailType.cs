using System.ComponentModel.DataAnnotations;

namespace Droits.Models;

public enum EmailType
{
    [Display(Name = "Report Acknowledged")]ReportAcknowledged,
    ReportCompleteInformation,
    ReportConfirmed,
    LloydsOfLondonNotification,
    ClosureOwnerNotFound,
    ClosureOwnerWaivesRights,
    ClosureCustom,
    CustomEmail,
    TestingDroitsv2
}
