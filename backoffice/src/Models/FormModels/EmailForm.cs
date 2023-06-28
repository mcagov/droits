using System.ComponentModel.DataAnnotations;
using Droits.Models.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Droits.Models.FormModels;
public class EmailForm
{
    public EmailForm()
    {

    }
    public EmailForm(Email email)
    {
        EmailId = email.Id;
        Recipient = email.Recipient;
        Subject = email.Subject;
        Body = email.Body;
    }
    public Guid EmailId { get; set; }
    [BindProperty]
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email address")]
    public string Recipient { get; set; } = string.Empty;
    [Required(ErrorMessage = "Subject is required")]
    public string Subject { get; set; } = string.Empty;
    [Required]
    [DataType(DataType.MultilineText)]
    public string Body { get; set; } = string.Empty;

    public Dictionary<string,dynamic> GetPersonalisation()
        => new (){
            { "subject", Subject},
            { "reference", "TestRef123"}
        };

    public string GetEmailBody()
    {
        var output = Body;
        foreach (var  param in GetPersonalisation())
        {
            output = output.Replace($"(({param.Key}))", param.Value);
        }

        return output;
    }

    public Email ApplyChanges(Email email)
    {
        email.Recipient = Recipient;
        email.Subject = Subject;
        email.Body = GetEmailBody();

        return email;
    }
}
