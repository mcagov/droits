#region

using System.ComponentModel;
using Droits.Models.Entities;
using Droits.Models.Enums;

#endregion

namespace Droits.Models.DTOs;

public class LetterExportDto
{
    public LetterExportDto()
    {
        
    }


    public LetterExportDto(Letter letter)
    {
        Recipient = letter.Recipient;
        DroitReference = letter.Droit?.Reference ?? string.Empty;
        QualityApprovedUser = letter.QualityApprovedUser?.Name;
        Status = letter.Status;
        Type = letter.Type;
    }
    
    // leave out id?
    public string Recipient { get; set; } = string.Empty;
    [DisplayName("Droit Reference")]
    public string DroitReference { get; set; } = string.Empty;
    [DisplayName("Quality Approved User")]
    public string QualityApprovedUser { get; set; } = string.Empty;
    [DisplayName("Letter Statusr")]
    public LetterStatus Status { get; set; }
    [DisplayName("Letter Type")]
    public LetterType Type { get; set; }
}