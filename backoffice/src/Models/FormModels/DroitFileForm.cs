#region

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Droits.Models.Entities;

#endregion

namespace Droits.Models.FormModels;

public class DroitFileForm : BaseEntityForm
{
    
    public DroitFileForm(){}
    public DroitFileForm(DroitFile droitFile) : base(droitFile)
    {
        Title = droitFile.Title;
        Url = droitFile.Url;
        WreckMaterialId = droitFile.WreckMaterialId;
        NoteId = droitFile.NoteId;

        Filename = droitFile.Filename;

    }


    public string? Title { get; set; }
    public string? Url { get; set; }
    public string? Filename { get; set; }

    public Guid? WreckMaterialId { get; set; }
    public Guid? NoteId { get; set; }

    [Required(ErrorMessage= "Please select a file to upload.")]
    [DisplayName("File Upload")]
    public IFormFile? DroitFile { get; set; } = null!;


    public DroitFile ApplyChanges(DroitFile droitFile)
    {
        base.ApplyChanges(droitFile);
        droitFile.Title = Title;
        droitFile.Url = Url;
        droitFile.WreckMaterialId = WreckMaterialId;
        droitFile.NoteId = NoteId;

        return droitFile;
    }
}