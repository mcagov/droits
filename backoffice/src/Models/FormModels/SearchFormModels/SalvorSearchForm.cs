using Droits.Models.FormModels.ExportFormModels;

namespace Droits.Models.FormModels.SearchFormModels;

public class SalvorSearchForm : SearchForm
{

    public SalvorSearchForm()
    {
        SalvorExportForm = new SalvorExportForm();
    }
    public string? Name { get; set; } = string.Empty;
    public string? Email { get; set; } = string.Empty;
    
    public SalvorExportForm SalvorExportForm { get; set; }
}