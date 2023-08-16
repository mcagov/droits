using System.ComponentModel.DataAnnotations;

namespace Droits.Models.Enums;

public enum NoteType
{
    [Display(Name = "General")]
    General,
    [Display(Name = "External Reference")]
    ExternalReference,
}