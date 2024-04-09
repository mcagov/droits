using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Droits.Models.FormModels;

public class WreckMaterialCsvForm
{
    public WreckMaterialCsvForm(){}


    public WreckMaterialCsvForm(Guid droitId,string droitRef)
    {
        DroitRef = droitRef;
        DroitId = droitId;
    }
    public Guid DroitId { get; set; }

    [DisplayName("Droit Reference")]
    public string DroitRef { get; set; } = string.Empty;
    
    [Required(ErrorMessage= "Please select a file to upload.")]
    [DisplayName("File Upload")]
    public IFormFile? CsvFile { get; set; }
}