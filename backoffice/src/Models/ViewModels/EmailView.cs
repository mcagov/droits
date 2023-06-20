using System.ComponentModel.DataAnnotations;
using System.Xml.Schema;
using Droits.Models.Entities;

namespace Droits.Models;

public class EmailView
{
    public EmailView(Email email)
    {
        Id = email.Id;
        Recipient = email.Recipient;
        EmailType = email.Type;
        DateLastModified = email.LastModified;
        Subject = email.Subject;
        Body = email.Body;
        SentDate = email.DateSent;
    }

    public Guid Id { get; }
    public string Recipient { get; } = string.Empty;
    public EmailType EmailType { get; }
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
    public DateTime DateLastModified { get; }
    public string Subject { get; } = string.Empty;
    public string Body { get; } = string.Empty;
    private DateTime? SentDate { get; }
    public bool IsSent => SentDate.HasValue;
}
