using System.ComponentModel;
using Droits.Models.Entities;

namespace Droits.Models.ViewModels;

public class AddressView
{
    public AddressView()
    {
    }

    public AddressView(Address address)
    {
        Line1 = address.Line1;
        Line2 = address.Line2;
        Town = address.Town;
        County = address.County;
        Postcode = address.Postcode;
    }

    [DisplayName("Line 1")]
    public string Line1 { get; } = string.Empty;


    [DisplayName("Line 2")]
    public string? Line2 { get; } = string.Empty;

    [DisplayName("City/Town")]
    public string Town { get; } = string.Empty;
    public string County { get; } = string.Empty;
    public string Postcode { get; } = string.Empty;
}
