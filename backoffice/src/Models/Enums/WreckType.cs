#region

using System.ComponentModel.DataAnnotations;

#endregion

namespace Droits.Models.Enums;

public enum WreckType
{
    [Display(Name = "Historic")]
    Historic,
    [Display(Name = "Modern")]
    Modern,
}