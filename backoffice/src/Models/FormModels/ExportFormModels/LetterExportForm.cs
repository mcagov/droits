using System.ComponentModel;

namespace Droits.Models.FormModels.ExportFormModels;

public class LetterExportForm
{
    public LetterExportForm()
    {
        
    }
    public bool Recipient { get; set; } = true;
    [DisplayName("Droit Reference")]
    public bool DroitReference { get; set; } = true;
    [DisplayName("Quality Approved User")]
    public bool QualityApprovedUser { get; set; } = true;
    [DisplayName("Wreck Type")]
    public bool Status { get; set; } = true;
    [DisplayName("Letter type")]
    public bool Type { get; set; } = true;

    public bool Subject { get; set; } = true;
}