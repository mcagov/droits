using System.ComponentModel.DataAnnotations;

namespace Droits.Models;

public enum EmailType
{
    [Display(Name = "Report Acknowledged")]
    ReportAcknowledged,
    [Display(Name = "Report Complete Information")]
    ReportCompleteInformation,
    [Display(Name = "Report Confirmed")]
    ReportConfirmed,
    [Display(Name = "Lloyds Of London Notification")]
    LloydsOfLondonNotification,
    [Display(Name = "Closure Owner Not Found")]
    ClosureOwnerNotFound,
    [Display(Name = "Closure Owner Waives Rights")]
    ClosureOwnerWaivesRights,
    [Display(Name = "Custom Closure")]
    ClosureCustom,
    [Display(Name = "Custom Email")]
    CustomEmail,
    [Display(Name = "Test Email")]
    TestingDroitsv2
}
