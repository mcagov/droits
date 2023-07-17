using Droits.Models.Enums;

namespace Droits.Models.Entities;

public class Wreck
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? VesselConstructionDetails { get; set; } = string.Empty;
    public int? VesselYearConstructed { get; set; }
    public DateTime? DateOfLoss { get; set; }
    public bool InUkWaters { get; set; } = false;
    public bool IsWarWreck { get; set; } = false;
    public bool IsAnAircraft { get; set; } = false;
    public string? Latitude { get; set; }
    public string? Longitude { get; set; }

    public bool IsProtectedSite { get; set; } = false;
    public string? ProtectionLegislation { get; set; }

    public string? AdditionalInformation { get; set; } = string.Empty;

    public DateTime Created { get; set; }
    public DateTime LastModified { get; set; }

    public List<Droit> Droits { get; set; } = new();
}