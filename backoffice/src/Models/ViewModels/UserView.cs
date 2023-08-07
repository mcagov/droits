using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Droits.Models.Entities;

namespace Droits.Models.ViewModels;

public class UserView
{
    public UserView()
    {
    }

    public UserView(ApplicationUser user, bool includeAssociations = false)
    {
        Id = user.Id;
        AuthId = user.AuthId;
        Name = user.Name;
        Email = user.Email;
        Created = user.Created;
        LastModified = user.LastModified;

        if (includeAssociations)
        {

        }
    }

    public Guid Id { get; }
    [DisplayName("Auth ID")]
    public string AuthId { get; } = string.Empty;
    public string Name { get; } = string.Empty;
    public string Email { get; } = string.Empty;

    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    public DateTime Created { get; }

    [DisplayName("Last Modified")]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    public DateTime LastModified { get; }

}