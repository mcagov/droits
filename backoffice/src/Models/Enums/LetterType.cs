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
    
    [Display(Name = "Closure Dredge Find Owner Not Found")]
    ClosureDredgeFindOwnerNotFound,
    
    [Display(Name = "Closure Dredge Find Owner Waives Rights")]
    ClosureDredgeFindOwnerWaivesRights,
    
    [Display(Name = "Closure Wessex Archaeology")]
    ClosureWessexArchaeology,

    [Display(Name = "Custom Closure")]
    ClosureCustom,
    
    [Display(Name = "Dredge Find EOD")]
    DredgeFindEod,

    [Display(Name = "Custom Letter")]
    CustomLetter,
    
    [Display(Name = "Custom Email")]
    CustomEmail,
    
    [Display(Name = "Closure Museum Letter Owner Found")]
    ClosureMuseumLetterOwnerFound,
    
    [Display(Name = "Closure Museum Letter Owner Not Found")]
    ClosureMuseumLetterOwnerNotFound,
    
    [Display(Name = "Closure Owner Found Museum Donation Agreed")]
    ClosureOwnerFoundMuseumDonationAgreed,
    
    [Display(Name = "Closure Owner Not Found But Museum Donation")]
    ClosureOwnerNotFoundButMuseumDonation
}