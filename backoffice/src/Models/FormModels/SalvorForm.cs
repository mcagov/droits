using System.ComponentModel.DataAnnotations;

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
    }

    public Guid Id { get; set; }
    [Required(ErrorMessage = "Name is required")]
    public string Name { get; set; } = string.Empty;
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email address")]
    public string Email { get; set; } = string.Empty;

    public Salvor ApplyChanges(Salvor salvor)
    {
        salvor.Id = Id;
        salvor.Name = Name;
        salvor.Email = Email;

        return salvor;
    }
}