using System.ComponentModel.DataAnnotations;
using System.Xml.Schema;
using Droits.Models.Domain;

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
        EmailType = "Default";
        DateLastModified = email.DateLastModified.ToString(format:"dd/MM/yyyy");
        Subject = email.Subject;
        Body = email.Body;
        SentDate = email.DateSent;
    }

    public Guid Id { get; }
    public string? Recipient { get; }
    public string? EmailType { get; }
    public string? DateLastModified { get; }
    public string? Subject { get; }
    public string? Body { get; }
    private DateTime? SentDate { get; }
    public bool IsSent => SentDate.HasValue;
}