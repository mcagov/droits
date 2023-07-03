using System.ComponentModel.DataAnnotations;
using Droits.Models.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Droits.Models.FormModels;

public class SalvorForm
{
    public SalvorForm()
    {
    }

    public SalvorForm(Salvor salvor)
    {
        Id = salvor.Id;
        Name = salvor.Name;
        Email = salvor.Email;
        TelephoneNumber = salvor.TelephoneNumber;
        DateOfBirth = salvor.DateOfBirth;
        _Address = new AddressForm(salvor.Address);
    }

    public Guid Id { get; set; }
    [Required(ErrorMessage = "Name is required")]
    public string Name { get; set; } = string.Empty;
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email address")]
    public string Email { get; set; } = string.Empty;
    [Required(ErrorMessage = "Phone number is required")]
    [Phone()]
    public string TelephoneNumber { get; set; } = string.Empty;

    [Required(ErrorMessage = "Date of Birth is required")]
    [BindProperty, DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    [DataType(DataType.Date)]
    public DateTime DateOfBirth { get; set; }

    public AddressForm _Address { get; set; } = new();

    public Salvor ApplyChanges(Salvor salvor)
    {
        salvor.Id = Id;
        salvor.Name = Name;
        salvor.Email = Email;
        salvor.DateOfBirth = DateOfBirth;
        salvor.Address.Line1 = _Address.Line1;
        salvor.Address.Line2 = _Address.Line2;
        salvor.Address.Town = _Address.Town;
        salvor.Address.County = _Address.County;
        salvor.Address.Postcode = _Address.Postcode;

        return salvor;
    }
}