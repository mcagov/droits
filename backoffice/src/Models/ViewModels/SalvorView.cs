#region

using System.ComponentModel;
using Droits.Models.Entities;
using Droits.Models.ViewModels.ListViews;

#endregion

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
        MobileNumber = salvor.MobileNumber;
        Address = new AddressView(salvor.Address);
        Notes = new NoteListView(salvor.Notes.Select(n => new NoteView(n)).OrderByDescending(n => n.Created).ToList());

        if ( includeAssociations )
        {
            Droits = new DroitListView(salvor.Droits.Select(d => new DroitView(d)).ToList());
        }
    }


    public Guid Id { get; }
    public string Email { get; } = string.Empty;
    public string Name { get; } = string.Empty;

    [DisplayName("Telephone Number")]
    public string? TelephoneNumber { get; } = string.Empty;
    
    [DisplayName("Mobile Number")]
    public string? MobileNumber { get; } = string.Empty;

    public AddressView Address { get; } = new();
    public DroitListView Droits { get; } = new();
    public NoteListView Notes { get; } = new();

}