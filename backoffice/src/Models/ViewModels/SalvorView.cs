using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Droits.Models.Entities;
using Droits.Models.ViewModels.ListViews;

namespace Droits.Models.ViewModels;

public class SalvorView : BaseEntityView
{
    public SalvorView()
    {
    }


    public SalvorView(Salvor salvor, bool includeAssociations = false) : base(salvor)
    {
        Id = salvor.Id;
        Email = salvor.Email;
        Name = salvor.Name;
        TelephoneNumber = salvor.TelephoneNumber;
        DateOfBirth = salvor.DateOfBirth;
        Address = new AddressView(salvor.Address);
        Notes = new NoteListView(salvor.Notes.Select(n => new NoteView(n)).OrderByDescending(n => n.LastModified).ToList());

        if ( includeAssociations )
        {
            Droits = new DroitListView(salvor.Droits.Select(d => new DroitView(d)).ToList());
        }
    }


    public Guid Id { get; }
    public string Email { get; } = string.Empty;
    public string Name { get; } = string.Empty;

    [DisplayName("Telephone Number")]
    public string TelephoneNumber { get; } = string.Empty;

    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    [DataType(DataType.Date)]
    [DisplayName("Date of Birth")]
    public DateTime DateOfBirth { get; }

    public AddressView Address { get; } = new();
    public DroitListView Droits { get; } = new();
    public NoteListView Notes { get; } = new();

}