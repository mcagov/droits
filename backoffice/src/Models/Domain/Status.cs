using System.ComponentModel.DataAnnotations;

namespace Droits.Models;

public enum DroitStatus
{
    Received,
    [Display(Name = "In Progress")]InProgress,
    Closed,
    Unassigned,
    [Display(Name = "Awaiting Response")]AwaitingResponse
}

public enum WreckStatus
{
    Active,
    Inactve
}
