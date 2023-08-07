namespace Droits.Models.Entities;

public class ApplicationUser
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;
    public string AuthId { get; set; } = string.Empty; // Azure AD Object ID (oid claim)
    public string Email { get; set; } = string.Empty;

    public DateTime Created { get; set; } = DateTime.UtcNow;
    public DateTime LastModified { get; set; } = DateTime.UtcNow;
    
    // ... Other properties as needed
}