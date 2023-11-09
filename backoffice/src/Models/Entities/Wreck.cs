namespace Droits.Models.Entities;

public class Wreck : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string? VesselConstructionDetails { get; set; } = string.Empty;
    public int? VesselYearConstructed { get; set; }
    public DateTime? DateOfLoss { get; set; }
    public bool InUkWaters { get; set; } = false;
    public bool IsWarWreck { get; set; } = false;
    public bool IsAnAircraft { get; set; } = false;
    public float? Latitude { get; set; }
    public float? Longitude { get; set; }

    public bool IsProtectedSite { get; set; } = false;
    public string? ProtectionLegislation { get; set; }
    
    public string? OwnerName { get; set; }
    public string? OwnerEmail { get; set; }
    public string? OwnerNumber { get; set; }

    public string? AdditionalInformation { get; set; } = string.Empty;

    public virtual ICollection<Droit> Droits { get; set; } = new List<Droit>();
    public virtual ICollection<Note> Notes { get; set; } = new List<Note>();
}