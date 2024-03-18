#region

using Droits.Models.Enums;
using Droits.Models.FormModels.ExportFormModels;

#endregion

namespace Droits.Models.FormModels.SearchFormModels;

public class LetterSearchForm : SearchForm
{

    public LetterSearchForm()
    {
        LetterExportForm = new LetterExportForm();
    }
    public string? Recipient { get; set; } = string.Empty;

    public List<LetterType> TypeList { get; set; } = new();
    public List<int> SelectedTypeList => TypeList.Select(s => ( int )s).ToList();    
    public List<LetterStatus> StatusList { get; set; } = new();
    public List<int> SelectedStatusList => StatusList.Select(s => ( int )s).ToList();
    
    public LetterExportForm LetterExportForm { get; set; }

}