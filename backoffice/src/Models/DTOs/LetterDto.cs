#region

using Droits.Models.Entities;
using Droits.Models.Enums;

#endregion

namespace Droits.Models.DTOs;

public class LetterDto
{
    public LetterDto()
    {
        
    }


    public LetterDto(Letter letter)
    {
        Recipient = letter.Recipient;
        DroitReference = letter.Droit.Reference;
        Status = letter.Status;
        Type = letter.Type;
    }
    
    // leave out id?
    public string Recipient { get; set; }
    public string DroitReference { get; set; }
    public LetterStatus Status { get; set; }
    public LetterType Type { get; set; }
}