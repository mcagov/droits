using System.ComponentModel.DataAnnotations;

namespace Droits.Models;

public class EmailForm
{
    [Editable(false)]
    public Guid? EmailId { get; set; }
    [Required]
    public string EmailAddress { get; set; } = string.Empty;
    [Required]
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

}
