using System.ComponentModel.DataAnnotations;

namespace Droits.Models.Enums;

public enum LetterStatus
{
    [Display(Name = "Draft")]
    Draft,
    [Display(Name = "Ready for QC")]
    ReadyForQC,
    [Display(Name = "Further Action Required")]
    FurtherActionRequired,
    [Display(Name = "QC Approved")]
    QCApproved
}