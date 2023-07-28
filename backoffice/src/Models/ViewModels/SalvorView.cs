using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Droits.Models.Entities;
using Droits.Models.ViewModels.ListViews;

namespace Droits.Models.ViewModels;

public class SalvorView
{
    public SalvorView()
    {
    }


    public SalvorView(Salvor salvor, bool includeAssociations = false)
    {
        Id = salvor.Id;
        Email = salvor.Email;
        Name = salvor.Name;
        TelephoneNumber = salvor.TelephoneNumber;
        DateOfBirth = salvor.DateOfBirth;
        Created = salvor.Created;
        LastModified = salvor.LastModified;
        Address = new AddressView(salvor.Address);

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

    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    public DateTime Created { get; }

    [DisplayName("Last Modified")]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    public DateTime LastModified { get; }
    
    public DroitListView Droits { get; } = new();

}