namespace Droits.Models;

public class EmailListView
{
    public EmailListView(List<EmailView> emails)
    {
        EmailList = emails;
    }
    public List<EmailView> EmailList { get; }
}