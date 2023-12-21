#region

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Droits.Models.Entities;

#endregion


namespace Droits.Models.FormModels;

public class SalvorForm : BaseEntityForm
{
    public SalvorForm()
    {
    }


    public SalvorForm(Salvor salvor) : base(salvor)
    {
        Name = salvor.Name;
        Email = salvor.Email;
        TelephoneNumber = salvor.TelephoneNumber;
        MobileNumber = salvor.MobileNumber;
        Address = new AddressForm(salvor.Address);
    }


    [Required(ErrorMessage = "Name is required")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email address")]
    public string Email { get; set; } = string.Empty;

    [DisplayName("Telephone Number")]
    public string? TelephoneNumber { get; set; } = string.Empty;
    
    [DisplayName("Mobile Number")]
    public string? MobileNumber { get; } = string.Empty;

    public AddressForm Address { get; set; } = new();


    public Salvor ApplyChanges(Salvor salvor)
    {
        base.ApplyChanges(salvor);
        
        salvor.Name = Name;
        salvor.Email = Email;
        salvor.TelephoneNumber = TelephoneNumber ?? "";
        salvor.MobileNumber = MobileNumber ?? "";

        Address.ApplyChanges(salvor.Address);

        return salvor;
    }
}