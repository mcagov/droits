using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Droits.Models.FormModels;

public class WreckMaterialCsvForm
{
    public WreckMaterialCsvForm(){}


    public WreckMaterialCsvForm(Guid droitId,String droitRef)
    {
        DroitRef = droitRef;
        DroitId = droitId;
    }


    public Guid DroitId { get; set; }

    [DisplayName("Droit Reference")]
    public String DroitRef { get; set; }
    
    [Required(ErrorMessage= "Please select a file to upload.")]
    [DisplayName("File Upload")]
    public IFormFile? CsvFile { get; set; } = null!;
}