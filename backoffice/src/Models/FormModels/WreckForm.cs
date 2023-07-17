using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Droits.Models.Entities;
using Droits.Models.Enums;

namespace Droits.Models.FormModels;

public class WreckForm : FormModel
{
    public WreckForm()
    {
    }


    public WreckForm(Wreck wreck)
    {
        Id = wreck.Id;
        Name = wreck.Name;

        VesselConstructionDetails = wreck.VesselConstructionDetails;
        VesselYearConstructed = wreck.VesselYearConstructed;

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

    public Guid Id { get; set; }

    [Required]
    public string Name { get; set; } = string.Empty;

    [DisplayName("Vessel Construction Details")]
    public string? VesselConstructionDetails { get; set; } = string.Empty;

    [DisplayName("Vessel Year Constructed")]
    public int? VesselYearConstructed { get; set; }


    [DisplayName("Date Of Loss")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    public DateTime? DateOfLoss { get; set; }

    [DisplayName("In UK Waters")]
    public bool InUkWaters { get; set; } = false;

    [DisplayName("Is A War Wreck")]
    public bool IsWarWreck { get; set; } = false;

    [DisplayName("Is An Aircraft")]
    public bool IsAnAircraft { get; set; } = false;

    public string? Latitude { get; set; }
    public string? Longitude { get; set; }

    [DisplayName("Is A Protected Site")]
    public bool IsProtectedSite { get; set; } = false;

    [DisplayName("Protection Legislation")]
    public string? ProtectionLegislation { get; set; }

    [DisplayName("Additional Information")]
    [DataType(DataType.MultilineText)]
    public string? AdditionalInformation { get; set; } = string.Empty;


    public Wreck ApplyChanges(Wreck wreck)
    {
        wreck.Id = Id;
        wreck.Name = Name;

        wreck.VesselConstructionDetails = VesselConstructionDetails;
        wreck.VesselYearConstructed = VesselYearConstructed;

        wreck.DateOfLoss = DateOfLoss;

        wreck.InUkWaters = InUkWaters;
        wreck.IsWarWreck = IsWarWreck;
        wreck.IsAnAircraft = IsAnAircraft;
        wreck.Latitude = Latitude;
        wreck.Longitude = Longitude;
        wreck.IsProtectedSite = IsProtectedSite;
        wreck.ProtectionLegislation = ProtectionLegislation;

        wreck.AdditionalInformation = AdditionalInformation;

        return wreck;
    }
}