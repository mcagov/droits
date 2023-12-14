#region

using System.ComponentModel.DataAnnotations;

#endregion

namespace Droits.Models.Enums;

public enum NoteType
{
    [Display(Name = "General")]
    General,
    [Display(Name = "External Reference")]
    ExternalReference,
}