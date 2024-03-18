using System.ComponentModel;
using Droits.Models.FormModels.ExportFormModels;

namespace Droits.Models.FormModels.SearchFormModels;

public class WreckSearchForm : SearchForm
{
    public WreckSearchForm()
    {
        WreckExportForm = new WreckExportForm();

    }
    [DisplayName("Verified Wreck Name")]
    public string? WreckName { get; set; } = string.Empty;
    
    [DisplayName("Owner Name")]
    public string? OwnerName { get; set; } = string.Empty;
    
    public WreckExportForm WreckExportForm { get; set; }
}