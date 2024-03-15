using System.ComponentModel;

namespace Droits.Models.FormModels.ExportFormModels;

public class SalvorExportForm
{
    public SalvorExportForm()
    {
        
    }
    public bool Created { get; set; } = true;
    public bool LastModified { get; set; } = true;
    public bool Name { get; set; } = true;
    public bool Email { get; set; } = true;
    [DisplayName("Telephone Number")]
    public bool TelephoneNumber { get; set; } = true;
    [DisplayName("Mobile Number")]
    public bool MobileNumber { get; set; } = true;
    [DisplayName("Address Postcode")]
    public bool AddressPostcode { get; set; } = true;
    [DisplayName("Address County")]
    public bool AddressCounty { get; set; } = true;
    [DisplayName("Address Town")]
    public bool AddressTown { get; set; } = true;
    [DisplayName("Address Line 1")]
    public bool AddressLine1 { get; set; } = true;
    [DisplayName("Address Line 2")]
    public bool AddressLine2 { get; set; } = true;
}