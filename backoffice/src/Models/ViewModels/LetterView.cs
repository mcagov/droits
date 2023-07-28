using System.ComponentModel.DataAnnotations;
using Droits.Models.Entities;
using Droits.Models.Enums;

namespace Droits.Models.ViewModels;

public class LetterView
{
    public LetterView(Letter letter)
    {
        Id = letter.Id;
        Recipient = letter.Recipient;
        LetterType = letter.Type;
        DateLastModified = letter.LastModified;
        Subject = letter.Subject;
        Body = letter.Body;
        SentDate = letter.DateSent;
    }


    public Guid Id { get; }
    public string Recipient { get; } = string.Empty;
    public LetterType LetterType { get; }

    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
    public DateTime DateLastModified { get; }

    public string Subject { get; } = string.Empty;
    public string Body { get; } = string.Empty;
    private DateTime? SentDate { get; }
    public bool IsSent => SentDate.HasValue;
}