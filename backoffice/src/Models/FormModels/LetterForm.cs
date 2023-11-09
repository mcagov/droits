using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Droits.Models.Entities;
using Droits.Models.Enums;
using Microsoft.AspNetCore.Mvc;

namespace Droits.Models.FormModels;

public class LetterForm : BaseEntityForm
{
    public LetterForm()
    {
    }

    public LetterForm(Letter letter) : base(letter)
    {
        DroitId = letter.DroitId;
        if ( letter.Droit != null )
        {
            DroitReference = letter.Droit.Reference;
        }
        Recipient = letter.Recipient;
        Subject = letter.Subject;
        Body = letter.Body;
        Status = letter.Status;
        Type = letter.Type;
    }


    [DisplayName("Droit Reference")]
    public string? DroitReference { get; set; }

    public Guid DroitId { get; set; }
    public LetterType Type { get; set; }

    [BindProperty]
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email address")]
    public string Recipient { get; set; } = string.Empty;

    [Required(ErrorMessage = "Subject is required")]
    public string Subject { get; set; } = string.Empty;

    [Required]
    [DataType(DataType.MultilineText)]
    public string Body { get; set; } = string.Empty;

    [DisplayName("Status")]
    public LetterStatus Status { get; set; }
    public Letter ApplyChanges(Letter letter)
    {
        
        base.ApplyChanges(letter);
        
        letter.DroitId = DroitId;
        letter.Recipient = Recipient;
        letter.Subject = Subject;
        letter.Body = Body;
        letter.Type = Type;
        letter.Status = Status;

        return letter;
    }
}
