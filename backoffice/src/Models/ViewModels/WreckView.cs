using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Droits.Models.Entities;
using Droits.Models.ViewModels.ListViews;

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
        Notes = new NoteListView(wreck.Notes.Select(n => new NoteView(n)).OrderByDescending(n => n.LastModified).ToList());

        if ( includeAssociations )
        {
            Droits = new DroitListView(wreck.Droits.Select(d => new DroitView(d)).ToList());
        }
    }


    // Base fields...

    public Guid Id { get; }
    public string Name { get; } = string.Empty;

    [DisplayName("Vessel Construction Details")]
    public string? VesselConstructionDetails { get; } = string.Empty;

    [DisplayName("Vessel Year Constructed")]
    public int? VesselYearConstructed { get; }

    [DisplayName("Date Of Loss")]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    public DateTime? DateOfLoss { get; }

    [DisplayName("In UK Waters")]
    public bool InUkWaters { get; } = false;

    [DisplayName("Is A War Wreck")]
    public bool IsWarWreck { get; } = false;

    [DisplayName("Is An Aircraft")]
    public bool IsAnAircraft { get; } = false;

    public float? Latitude { get; }
    public float? Longitude { get; }

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


    [DisplayName("Additional Information")]
    [DataType(DataType.MultilineText)]
    public string? AdditionalInformation { get; } = string.Empty;

    public DroitListView Droits { get; } = new();
    public NoteListView Notes { get; } = new();

}