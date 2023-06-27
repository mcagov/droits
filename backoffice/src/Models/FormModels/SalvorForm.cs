using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace Droits.Models;

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
    }

    public Guid Id { get; set; }
    [Required(ErrorMessage = "Name is required")]
    public string Name { get; set; } = string.Empty;
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email address")]
    public string Email { get; set; } = string.Empty;

    public string TelephoneNumber { get; set; } = string.Empty;

    [Required(ErrorMessage = "Date of Birth is required")]
    [BindProperty, DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    [DataType(DataType.Date)]
    public DateTime DateOfBirth { get; set; } 

    public Salvor ApplyChanges(Salvor salvor)
    {
        salvor.Id = Id;
        salvor.Name = Name;
        salvor.Email = Email;
        salvor.DateOfBirth = DateOfBirth;

        return salvor;
    }
}