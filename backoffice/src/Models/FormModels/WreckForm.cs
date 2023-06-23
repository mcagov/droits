using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Droits.Models;

public class WreckForm : FormModel
{
    public WreckForm()
    {
    }

    public WreckForm(Wreck wreck)
    {
        Id = wreck.Id;
        Status = wreck.Status;
        Name = wreck.Name;

        DateOfLoss = wreck.DateOfLoss;
        IsWarWreck = wreck.IsWarWreck;
        IsAnAircraft = wreck.IsAnAircraft;
        Latitude = wreck.Latitude;
        Longitude = wreck.Longitude;
        IsProtectedSite = wreck.IsProtectedSite;
        ProtectionLegislation = wreck.ProtectionLegislation;

    }


    // Base fields...

    public Guid Id { get; set; }
    public WreckStatus Status { get; set; }
    [Required]
    public string Name { get; set; } = string.Empty;


    [DisplayName("Date Of Loss")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    public DateTime? DateOfLoss { get; set; }

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

    public Wreck ApplyChanges(Wreck wreck)
    {

        wreck.Id = Id;
        wreck.Status = Status;
        wreck.Name = Name;

        wreck.DateOfLoss = DateOfLoss;
        wreck.IsWarWreck = IsWarWreck;
        wreck.IsAnAircraft = IsAnAircraft;
        wreck.Latitude = Latitude;
        wreck.Longitude = Longitude;
        wreck.IsProtectedSite = IsProtectedSite;
        wreck.ProtectionLegislation = ProtectionLegislation;

        return wreck;
    }
}
