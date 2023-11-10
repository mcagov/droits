#region

using System.ComponentModel.DataAnnotations;

#endregion

namespace Droits.Models.Enums;

public enum LetterStatus
{
    [Display(Name = "Draft")]
    Draft,
    [Display(Name = "Ready for QC")]
    ReadyForQC,
    [Display(Name = "Action Required")]
    ActionRequired,
    [Display(Name = "QC Approved")]
    QCApproved,
    [Display(Name = "Sent")]
    Sent
    
}