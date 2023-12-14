#region

using System.ComponentModel.DataAnnotations;

#endregion

namespace Droits.Models.Enums;

public enum LetterType
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

    [Display(Name = "Custom Letter")]
    CustomLetter
}