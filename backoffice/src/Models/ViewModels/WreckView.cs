using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Droits.Models.Entities;

namespace Droits.Models.ViewModels;

public class WreckView
{
    public WreckView()
    {
    }

    public WreckView(Wreck wreck)
    {
        Id = wreck.Id;
        Name = wreck.Name;

        VesselConstructionDetails = wreck.VesselConstructionDetails;
        VesselYearConstructed = wreck.VesselYearConstructed;
        Created = wreck.Created;
        LastModified = wreck.LastModified;


        DateOfLoss = wreck.DateOfLoss;
        InUkWaters = wreck.InUkWaters;
        IsWarWreck = wreck.IsWarWreck;
        IsAnAircraft = wreck.IsAnAircraft;
        Latitude = wreck.Latitude;
        Longitude = wreck.Longitude;
        IsProtectedSite = wreck.IsProtectedSite;
        ProtectionLegislation = wreck.ProtectionLegislation;
        AdditionalInformation = wreck.AdditionalInformation;
    }


    // Base fields...

    public Guid Id { get; }
    public string Name { get; } = string.Empty;

    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    public DateTime Created { get; }

    [DisplayName("Last Modified")]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    public DateTime LastModified { get; }

    [DisplayName("Vessel Construction Details")]
    public string? VesselConstructionDetails { get; } = string.Empty;

    [DisplayName("Vessel Year Constructed")]
    public int? VesselYearConstructed { get; }

    [DisplayName("Date Of Loss")]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    public DateTime? DateOfLoss { get; }

    [DisplayName("In UK Waters")]
    public bool InUkWaters { get; }

    [DisplayName("Is A War Wreck")]
    public bool IsWarWreck { get; }

    [DisplayName("Is An Aircraft")]
    public bool IsAnAircraft { get; }

    public string? Latitude { get; }
    public string? Longitude { get; }

    [DisplayName("Is A Protected Site")]
    public bool IsProtectedSite { get; }

    [DisplayName("Protection Legislation")]
    public string? ProtectionLegislation { get; }


    [DisplayName("Additional Information")]
    [DataType(DataType.MultilineText)]
    public string? AdditionalInformation { get; } = string.Empty;
}