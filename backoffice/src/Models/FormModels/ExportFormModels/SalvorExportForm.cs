using System.ComponentModel;

namespace Droits.Models.FormModels.ExportFormModels;

public class SalvorExportForm
{
    public SalvorExportForm()
    {
        
    }
    public bool Created { get; set; } = true;
    [DisplayName("Last Modified")]
    public bool LastModified { get; set; } = true;
    public bool Name { get; set; } = true;
    public bool Email { get; set; } = true;
    [DisplayName("Telephone Number")]
    public bool TelephoneNumber { get; set; } = true;
    [DisplayName("Mobile Number")]
    public bool MobileNumber { get; set; } = true;
    [DisplayName("Address")]
    public bool Address { get; set; } = true;

    [DisplayName("Droit Count")]
    public bool DroitCount { get; set; } = true;

    [DisplayName("Droit References")]
    public bool DroitRefs { get; set; } = true;
}