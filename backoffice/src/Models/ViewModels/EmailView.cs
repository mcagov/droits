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
        Date = email.DateLastModified;
        Subject = email.Subject;
        SentDate = email.DateSent;
    }

    public Guid Id { get; set; }
    public string? Recipient { get; set; }
    public string? EmailType { get; set; }
    [Display(Name = "Date")]
    [DataType(DataType.Date)]
    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
    public DateTime Date { get; set; }
    public string? Subject { get; set; }
    public DateTime? SentDate { get; set; }
    public bool IsSent => SentDate.HasValue;
}