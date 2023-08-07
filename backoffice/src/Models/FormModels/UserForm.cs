using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Droits.Models.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Droits.Models.FormModels;

public class UserForm
{
    public UserForm()
    {
    }

    public UserForm(ApplicationUser user)
    {
        Id = user.Id;
        AuthId = user.AuthId;
        Name = user.Name;
        Email = user.Email;
    }

    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Auth ID is required")]
    
    [DisplayName("Auth ID")]
    public string AuthId { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email address")]
    public string Email { get; set; } = string.Empty;

    public ApplicationUser ApplyChanges(ApplicationUser user)
    {
        user.Id = Id;
        user.AuthId = AuthId;
        user.Name = Name;
        user.Email = Email;

        return user;
    }
}