using System.ComponentModel.DataAnnotations;
using System.Xml.Schema;
using Droits.Models.Entities;

namespace Droits.Models;

public class EmailView
{
    public EmailView()
    {
    }

    public EmailView(Email email)
    {
        Id = email.Id;
        Recipient = email.Recipient;
        EmailType = email.Type;
        DateLastModified = email.DateLastModified;
        Subject = email.Subject;
        Body = email.Body;
        SentDate = email.DateSent;
    }

    public Guid Id { get; }
    public string Recipient { get; }
    public EmailType EmailType { get; }
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
    public DateTime DateLastModified { get; }
    public string Subject { get; }
    public string Body { get; }
    private DateTime? SentDate { get; }
    public bool IsSent => SentDate.HasValue;
}