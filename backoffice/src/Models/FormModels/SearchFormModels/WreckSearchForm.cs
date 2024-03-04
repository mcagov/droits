using System.ComponentModel;

namespace Droits.Models.FormModels.SearchFormModels;

public class WreckSearchForm : SearchForm
{
    [DisplayName("Verified Wreck Name")]
    public string? WreckName { get; set; } = string.Empty;
    
    [DisplayName("Owner Name")]
    public string? OwnerName { get; set; } = string.Empty;
}