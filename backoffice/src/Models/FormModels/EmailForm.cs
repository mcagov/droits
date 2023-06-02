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
            { "body", Body},
            { "subject", Subject}
        };
}
