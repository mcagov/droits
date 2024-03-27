#region

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Droits.Models.Entities;
using Droits.Models.Enums;

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
        WreckType = wreck.WreckType;
        
        ConstructionDetails = wreck.ConstructionDetails;
        YearConstructed = wreck.YearConstructed;

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
        OwnerAddress = wreck.OwnerAddress;
    }


    [Required]
    [DisplayName("Verified Wreck Name")]
    public string Name { get; set; } = string.Empty;
    [DisplayName("Wreck Type")]
    public WreckType? WreckType { get; set; }
    
    [DisplayName("Construction Details")]
    public string? ConstructionDetails { get; set; } = string.Empty;

    [DisplayName("Year Constructed")]
    public int? YearConstructed { get; set; }


    [DisplayName("Date Of Loss")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    public DateTime DateOfLoss { get; set; }

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
    
    [DisplayName("Additional Information")]
    [DataType(DataType.MultilineText)]
    public string? AdditionalInformation { get; set; } = string.Empty;
    
    [DisplayName("Owner Name")]
    public string? OwnerName { get; set; }
    [DisplayName("Owner Email")]
    [EmailAddress(ErrorMessage = "Invalid email")]
    public string? OwnerEmail { get; set; }
    [DisplayName("Owner Number")]
    [Phone(ErrorMessage = "Invalid phone number")]
    public string? OwnerNumber { get; set; }

    [DisplayName("Owner Address")]
    [DataType(DataType.MultilineText)]
    public string? OwnerAddress { get; set; } = string.Empty;

    
    public IEnumerable<string> ProtectionLegislationList => new List<string>() { "Protected Wreck Act 1973", "AMAAA 1979", "Protection of Military Remains Act 1986", "Scotland Historic Marine Protected Areas"};

    public Wreck ApplyChanges(Wreck wreck)
    {
        
        base.ApplyChanges(wreck);
        
        wreck.Name = Name;
        wreck.WreckType = WreckType;
        wreck.ConstructionDetails = ConstructionDetails;
        wreck.YearConstructed = YearConstructed;

        wreck.DateOfLoss = DateOfLoss;

        wreck.InUkWaters = InUkWaters;
        wreck.IsWarWreck = IsWarWreck;
        wreck.IsAnAircraft = IsAnAircraft;
        wreck.Latitude = Latitude;
        wreck.Longitude = Longitude;
        wreck.IsProtectedSite = IsProtectedSite;
        wreck.ProtectionLegislation = ProtectionLegislation;
        wreck.AdditionalInformation = AdditionalInformation;


        wreck.OwnerName = OwnerName;
        wreck.OwnerEmail = OwnerEmail;
        wreck.OwnerNumber = OwnerNumber;
        wreck.OwnerAddress = OwnerAddress;


        return wreck;
    }
}