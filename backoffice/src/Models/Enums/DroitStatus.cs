using System.ComponentModel.DataAnnotations;

namespace Droits.Models.Enums;

public enum DroitStatus
{
    [Display(Name = "Unassigned")]
    Unassigned,

    [Display(Name = "Received")]
    Received,

    [Display(Name = "In Progress")]
    InProgress,

    [Display(Name = "Research")]
    Research,

    [Display(Name = "Closed")]
    Closed,

    [Display(Name = "Awaiting Response")]
    AwaitingResponse,
}
