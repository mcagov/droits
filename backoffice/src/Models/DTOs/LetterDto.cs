using Droits.Models.Entities;
using Droits.Models.Enums;

namespace Droits.Models.DTOs;

public class LetterDto
{
    public LetterDto()
    {
        
    }


    public LetterDto(Letter letter)
    {
        Recipient = letter.Recipient;
        DroitReference = letter.Droit?.Reference ?? string.Empty;
        Status = letter.Status;
        Type = letter.Type;
    }
    
    // leave out id?
    public string Recipient { get; set; } = string.Empty;
    public string DroitReference { get; set; } = string.Empty;
    public LetterStatus Status { get; set; }
    public LetterType Type { get; set; }
}