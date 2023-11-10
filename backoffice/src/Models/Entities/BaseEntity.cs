#region

using System.ComponentModel.DataAnnotations.Schema;

#endregion

namespace Droits.Models.Entities;

public class BaseEntity
{
    public Guid Id { get; set; }
    public DateTime Created { get; set; } = DateTime.UtcNow;
    public DateTime LastModified { get; set; } = DateTime.UtcNow;
    
    public Guid LastModifiedByUserId { get; set; }
    [ForeignKey("LastModifiedByUserId")]
    public virtual ApplicationUser? LastModifiedByUser { get; set; } = null!;
}