#region

using System.ComponentModel.DataAnnotations;

#endregion

namespace Droits.Models.Enums;

public enum DroitStatus
{
    [Display(Name = "Received")]
    Received,

    [Display(Name = "Acknowledged")]
    AcknowledgementLetterSent,

    [Display(Name = "Initial Research")]
    InitialResearch,

    [Display(Name = "Research")]
    Research,

    [Display(Name = "Salvage Award")]
    NegotiatingSalvageAward,

    [Display(Name = "Ready For QC")]
    ReadyForQc,

    [Display(Name = "QC Approved")]
    QcApproved,

    [Display(Name = "Closed")]
    Closed
}