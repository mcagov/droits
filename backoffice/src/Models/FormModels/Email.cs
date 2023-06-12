namespace Droits.Models;

public class Email
{
    public Guid Id { get; set; }
    public string? Subject { get; set; }
    public string Body { get; set; }
    public string SenderEmailAddress { get; set; }
    public DateTime DateSent { get; set; }
}