using Droits.Models.Enums;

namespace Droits.Models.Entities;

public class Letter
{
    public Guid Id { get; set; }
    public string Subject { get; set; } = string.Empty;
    public string Body { get; set; } = string.Empty;
    public string Recipient { get; set; } = string.Empty;
    public LetterType Type { get; set; } = LetterType.CustomLetter;
    public Guid SenderUserId { get; set; }
    public DateTime? DateSent { get; set; }
    public DateTime Created { get; set; }
    public DateTime LastModified { get; set; }
}