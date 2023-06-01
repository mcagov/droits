using System.ComponentModel.DataAnnotations;

namespace Droits.Models;

public class EmailForm
{
    public string EmailAddress { get; set; } = string.Empty;
    public string Subject { get; set; } = string.Empty;

    [DataType(DataType.MultilineText)] public string Body { get; set; } = string.Empty;

    public Dictionary<string, dynamic> GetPersonalisation()
    {
        return new Dictionary<string, dynamic>
        {
            { "reference", "123" },
            { "custom message", Subject },
            { "hazardous find", "yes" },
            { "mmo", "yes" },
            { "wreck add info", "yes" },
            { "item add info", "yes" },
            { "find add info", "yes" },
            { "rip", "yes" },
            { "rip no recover", "yes" },
            { "archaeological protocol", "yes" },
            { "more info", "yes" }
        };
    }
}