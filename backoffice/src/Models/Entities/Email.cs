using Droits.Models.Enums;

namespace Droits.Models.Entities;

public class Email
{
    public Guid Id { get; set; }
    public string Subject { get; set; } = string.Empty;
    public string Body { get; set; } = string.Empty;
    public string Recipient { get; set; } = string.Empty;
    public EmailType Type { get; set; } = EmailType.CustomEmail;
    public Guid SenderUserId { get; set; }
    public DateTime? DateSent { get; set; }
    public DateTime Created { get; set; }
    public DateTime LastModified { get; set; }
}