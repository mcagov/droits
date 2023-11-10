namespace Droits.Models.Entities;

public class Salvor : BaseEntity
{
    public string Email { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string TelephoneNumber { get; set; } = string.Empty;
    public Address Address { get; set; } = new();
    public virtual ICollection<Droit> Droits { get; set; } = new List<Droit>();
    public virtual ICollection<Note> Notes { get; set; } = new List<Note>();
}