#region

using System.ComponentModel.DataAnnotations;

#endregion

namespace Droits.Models.Enums;

public enum WreckMaterialOutcome
{
    
    [Display(Name = "Lieu of Salvage")]
    LieuOfSalvage,
    [Display(Name = "Returned to Owner")]
    ReturnedToOwner,
    [Display(Name = "Donated to Museum")]
    DonatedToMuseum,
    [Display(Name = "Sold to Museum")]
    SoldToMuseum,
    [Display(Name = "Other")]
    Other,
}