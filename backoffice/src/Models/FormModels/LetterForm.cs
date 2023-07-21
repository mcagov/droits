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


    public LetterForm(Guid droitId)
    {
        DroitId = droitId;
    }


    public LetterForm(Letter letter)
    {
        LetterId = letter.Id;
        Recipient = letter.Recipient;
        Subject = letter.Subject;
        Body = letter.Body;
    }


    public Guid LetterId { get; set; }
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


    public Dictionary<string, dynamic> GetPersonalisation()
    {
        return new Dictionary<string, dynamic>
        {
            { "email address", Recipient},
            { "subject", Subject },
            { "reference", "TestRef123" },
            { "custom message", "message le custom" },
            { "item pluralised", "*item pluralised*" },
            { "items", "*items*" },
            { "this pluralised", "*this pluralised*" },
            { "wreck", "*wreck name*" },
            { "date", "*date*" },
            { "has pluralised", "*has pluralised*" },
            { "link_to_file", "*link to file*" },
            { "is pluralised", "*is pluralised*" }
        };
    }


    public string GetLetterBody()
    {
        var output = Body;
        foreach ( var param in GetPersonalisation() )
        {
            output = output.Replace($"(({param.Key}))", param.Value);
        }

        return output;
    }


    public Letter ApplyChanges(Letter letter)
    {
        letter.Recipient = Recipient;
        letter.Subject = Subject;
        letter.Body = GetLetterBody();
        letter.DroitId = DroitId;
        letter.Type = Type;

        return letter;
    }
}