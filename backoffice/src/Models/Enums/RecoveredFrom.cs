#region

using System.ComponentModel.DataAnnotations;

#endregion

namespace Droits.Models.Enums;

public enum RecoveredFrom
{
    [Display(Name = "Shipwreck")]
    Shipwreck,
        
    [Display(Name = "Seabed")]
    Seabed,
        
    [Display(Name = "Afloat")]
    Afloat,
        
    [Display(Name = "Sea Shore")]
    SeaShore,
}