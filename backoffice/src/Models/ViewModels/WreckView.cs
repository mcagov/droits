using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Droits.Models.Entities;
using Droits.Models.Enums;

namespace Droits.Models.ViewModels;
public class WreckView
{
    public WreckView()
    {
    }

    public WreckView(Wreck wreck)
    {
        Id = wreck.Id;
        Status = wreck.Status;
        Name = wreck.Name;
        Created = wreck.Created;
        LastModified = wreck.LastModified;


        DateOfLoss = wreck.DateOfLoss;
        IsWarWreck = wreck.IsWarWreck;
        IsAnAircraft = wreck.IsAnAircraft;
        Latitude = wreck.Latitude;
        Longitude = wreck.Longitude;
        IsProtectedSite = wreck.IsProtectedSite;
        ProtectionLegislation = wreck.ProtectionLegislation;
    }


    // Base fields...

    public Guid Id { get; }
    public WreckStatus Status { get; }
    public string Name { get; } = string.Empty;

    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    public DateTime Created { get; }

    [DisplayName("Last Modified")]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    public DateTime LastModified { get; }

    [DisplayName("Date Of Loss")]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    public DateTime? DateOfLoss { get; }

    [DisplayName("Is A War Wreck")]
    public bool IsWarWreck { get; } = false;

    [DisplayName("Is An Aircraft")]
    public bool IsAnAircraft { get; } = false;
    public string? Latitude { get; }
    public string? Longitude { get; }

    [DisplayName("Is A Protected Site")]
    public bool IsProtectedSite { get; } = false;

    [DisplayName("Protection Legislation")]
    public string? ProtectionLegislation { get; }
}
