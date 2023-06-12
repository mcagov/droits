using System.ComponentModel.DataAnnotations;

namespace Droits.Models;

public class EmailForm
{
    public string EmailAddress { get; set; } = string.Empty;
    public string Subject { get; set; } = string.Empty;

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
