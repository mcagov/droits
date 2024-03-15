using System.ComponentModel;

namespace Droits.Models.FormModels.ExportFormModels;

public class WreckExportForm
{
    public WreckExportForm()
    {
        
    }
    public bool Created { get; set; } = true;
    public bool LastModified { get; set; } = true;
    public bool Name { get; set; } = true;
    [DisplayName("Wreck Type")]
    public bool Type { get; set; } = true;
    [DisplayName("Construction Details")]
    public bool ConstructionDetails { get; set; } = true;
    [DisplayName("Year Constructed")]
    public bool YearConstructed { get; set; } = true;
    [DisplayName("Date of Loss")]
    public bool DateOfLoss { get; set; } = true;
    [DisplayName("In Uk Waters?")]
    public bool InUkWaters { get; set; } = true;
    [DisplayName("Is War Wreck?")]
    public bool IsWarWreck { get; set; } = true;
    [DisplayName("Is An Aircraft?")]
    public bool IsAnAircraft { get; set; } = true;
    public bool Latitude { get; set; } = true;
    public bool Longitude { get; set; } = true;
    [DisplayName("Is Protected Site?")]
    public bool IsProtectedSite { get; set; } = true;
    [DisplayName("Protection Legislation")]
    public bool ProtectionLegislation { get; set; } = true;
    [DisplayName("Additional Information")]
    public bool AdditionalInformation { get; set; } = true;
    [DisplayName("Owner Name")]
    public bool OwnerName { get; set; } = true;
    [DisplayName("Owner Email")]
    public bool OwnerEmail { get; set; } = true;
    [DisplayName("Owner Number")]
    public bool OwnerNumber { get; set; } = true;
    [DisplayName("Owner Address")]
    public bool OwnerAddress{ get; set; } = true;
}