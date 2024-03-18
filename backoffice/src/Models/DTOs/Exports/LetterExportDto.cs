#region

using System.ComponentModel;
using Droits.Models.Entities;
using Droits.Models.Enums;

#endregion

namespace Droits.Models.DTOs.Exports;

public class LetterExportDto
{

    public LetterExportDto(Letter letter)
    {
        Recipient = letter.Recipient;
        DroitReference = letter.Droit?.Reference ?? string.Empty;
        QualityApprovedUser = letter.QualityApprovedUser?.Name ?? "N/A";
        Status = letter.Status;
        Type = letter.Type;
    }
    
    public string? Recipient { get; set; }
    [DisplayName("Droit Reference")]
    public string DroitReference { get; set; }
    [DisplayName("Quality Approved User")]
    public string QualityApprovedUser { get; set; }
    [DisplayName("Status")]
    public LetterStatus Status { get; set; }
    [DisplayName("Letter Type")]
    public LetterType Type { get; set; }

    public object Subject { get; set; }
}