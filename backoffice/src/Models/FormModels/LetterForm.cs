using System.ComponentModel.DataAnnotations;
using Droits.Models.Entities;
using Droits.Models.Enums;
using Microsoft.AspNetCore.Mvc;

namespace Droits.Models.FormModels;

public class LetterForm
{
    public LetterForm()
    {
    }

    public LetterForm(Letter letter)
    {
        Id = letter.Id;
        DroitId = letter.DroitId;
        Recipient = letter.Recipient;
        Subject = letter.Subject;
        Body = letter.Body;
    }


    public Guid Id { get; set; }
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
    public Letter ApplyChanges(Letter letter)
    {
        letter.Id = Id;
        letter.DroitId = DroitId;
        letter.Recipient = Recipient;
        letter.Subject = Subject;
        letter.Body = Body;
        letter.Type = Type;

        return letter;
    }
}
