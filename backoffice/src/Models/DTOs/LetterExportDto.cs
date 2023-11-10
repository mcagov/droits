#region

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
    public string DroitReference { get; set; } = string.Empty;
    public string QualityApprovedUser { get; set; } = string.Empty;
    public LetterStatus Status { get; set; }
    public LetterType Type { get; set; }
}