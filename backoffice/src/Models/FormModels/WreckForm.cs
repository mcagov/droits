#region

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Droits.Models.Entities;

#endregion

namespace Droits.Models.FormModels;

public class WreckForm : BaseEntityForm
{
    public WreckForm()
    {
    }


    public WreckForm(Wreck wreck) : base(wreck)
    {
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

        OwnerName = wreck.OwnerName;
        OwnerEmail = wreck.OwnerEmail;
        OwnerNumber = wreck.OwnerNumber;
    }


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

    public double? Latitude { get; set; }
    public double? Longitude { get; set; }

    [DisplayName("Is A Protected Site")]
    public bool IsProtectedSite { get; set; } = false;

    
    [DisplayName("Protection Legislation")]
    public string? ProtectionLegislation { get; set; }
    [DisplayName("Owner Name")]
    public string? OwnerName { get; set; }
    [DisplayName("Owner Email")]
    [EmailAddress(ErrorMessage = "Invalid email")]
    public string? OwnerEmail { get; set; }
    [DisplayName("Owner Number")]
    [Phone(ErrorMessage = "Invalid phone number")]
    public string? OwnerNumber { get; set; }

    [DisplayName("Additional Information")]
    [DataType(DataType.MultilineText)]
    public string? AdditionalInformation { get; set; } = string.Empty;

    
    public IEnumerable<string> ProtectionLegislationList => new List<string>() { "Protected Wreck Act 1973", "AMAAA 1979", "Protection of Military Remains Act 1986", "Scotland Historic Marine Protected Areas"};

    public Wreck ApplyChanges(Wreck wreck)
    {
        
        base.ApplyChanges(wreck);
        
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

        wreck.OwnerName = OwnerName;
        wreck.OwnerEmail = OwnerEmail;
        wreck.OwnerNumber = OwnerNumber;

        wreck.AdditionalInformation = AdditionalInformation;

        return wreck;
    }
}