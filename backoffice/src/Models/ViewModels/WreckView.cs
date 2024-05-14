#region

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Droits.Models.Entities;
using Droits.Models.Enums;
using Droits.Models.ViewModels.ListViews;

#endregion

namespace Droits.Models.ViewModels;

public class WreckView : BaseEntityView
{
    public WreckView()
    {
    }


    public WreckView(Wreck wreck, bool includeAssociations = false) : base(wreck)
    {
        Id = wreck.Id;
        Name = wreck.Name;
        WreckType = wreck.WreckType;
        
        ConstructionDetails = wreck.ConstructionDetails;
        YearConstructed = wreck.YearConstructed;

        DateOfLoss = wreck.DateOfLoss?.ToUniversalTime();
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
        Notes = new NoteListView(wreck.Notes.Select(n => new NoteView(n)).OrderByDescending(n => n.Created).ToList());

        if ( includeAssociations )
        {
            Droits = new DroitListView(wreck.Droits.Select(d => new DroitView(d)).ToList());
        }
    }


    // Base fields...

    public Guid Id { get; }
    
    [DisplayName("Verified Wreck Name")]
    public string Name { get; } = string.Empty;
    
    [DisplayName("Wreck Type")]
    public WreckType? WreckType { get; set; }

    [DisplayName("Construction Details")]
    public string? ConstructionDetails { get; } = string.Empty;

    [DisplayName("Year Constructed")]
    public int? YearConstructed { get; }

    [DisplayName("Date Of Loss")]
    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
    public DateTime? DateOfLoss { get; }

    [DisplayName("In UK Waters")]
    public bool InUkWaters { get; } = false;

    [DisplayName("Is A War Wreck")]
    public bool IsWarWreck { get; } = false;

    [DisplayName("Is An Aircraft")]
    public bool IsAnAircraft { get; } = false;

    public double? Latitude { get; }
    public double? Longitude { get; }

    [DisplayName("Is A Protected Site")]
    public bool IsProtectedSite { get; } = false;

    [DisplayName("Protection Legislation")]
    public string? ProtectionLegislation { get; }

    [DisplayName("Owner Number")]
    public string? OwnerNumber { get; }

    [DisplayName("Owner Email")]
    public string? OwnerEmail { get; }

    [DisplayName("Owner Name")]
    public string? OwnerName { get; }
    
    [DataType(DataType.MultilineText)]
    [DisplayName("Owner Address")]
    public string? OwnerAddress { get; }


    [DisplayName("Additional Information")]
    [DataType(DataType.MultilineText)]
    public string? AdditionalInformation { get; } = string.Empty;

    public DroitListView Droits { get; } = new();
    public NoteListView Notes { get; } = new();

}