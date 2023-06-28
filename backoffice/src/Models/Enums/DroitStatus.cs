using System.ComponentModel.DataAnnotations;

namespace Droits.Models.Enums;

public enum DroitStatus
{
    Received,
    [Display(Name = "In Progress")]InProgress,
    Closed,
    Unassigned,
    [Display(Name = "Awaiting Response")]AwaitingResponse
}
