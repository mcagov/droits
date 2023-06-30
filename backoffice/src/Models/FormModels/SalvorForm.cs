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
        AddressLine1 = salvor.Address.Line1;
        AddressLine2 = salvor.Address.Line2;
        AddressTown = salvor.Address.Town;
        AddressCounty = salvor.Address.County;
        AddressPostcode = salvor.Address.Postcode;
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
    public string AddressLine1 { get; set; } = string.Empty;
    public string AddressLine2 { get; set; } = string.Empty;
    public string AddressTown { get; set; } = string.Empty;
    public string AddressCounty { get; set; } = string.Empty;
    [Required(ErrorMessage = "Postcode is required")]
    [RegularExpression("([Gg][Ii][Rr] 0[Aa]{2})|((([A-Za-z][0-9]{1,2})" +
                       "|(([A-Za-z][A-Ha-hJ-Yj-y][0-9]{1,2})|(([AZa-z][0-9][A-Za-z])" +
                       "|([A-Za-z][A-Ha-hJ-Yj-y][0-9]?[A-Za-z]))))[0-9][A-Za-z]{2})",
        ErrorMessage = "Invalid postcode")]
    public string AddressPostcode { get; set; } = string.Empty;

    public Salvor ApplyChanges(Salvor salvor)
    {
        salvor.Id = Id;
        salvor.Name = Name;
        salvor.Email = Email;
        salvor.DateOfBirth = DateOfBirth;
        salvor.Address.Line1 = AddressLine1;
        salvor.Address.Line2 = AddressLine2;
        salvor.Address.Town = AddressTown;
        salvor.Address.County = AddressCounty;
        salvor.Address.Postcode = AddressPostcode;

        return salvor;
    }
}