namespace Droits.Models.Domain;

public class Email
{
    public Guid Id { get; set; }
    public string Subject { get; set; }
    public string Body { get; set; }
    public string Recipient { get; set; }

    public Guid SenderUserId { get; set; }
    public DateTime? DateSent { get; set; }
    public DateTime DateCreated { get; set; }
    public DateTime DateLastModified { get; set; }
}