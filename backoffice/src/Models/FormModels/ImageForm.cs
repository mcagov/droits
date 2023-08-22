using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Droits.Models.Entities;

namespace Droits.Models.FormModels;

public class ImageForm : BaseEntityForm
{
    public ImageForm()
    {
        
    }


    public ImageForm(Image image) : base(image)
    {
        Title = image.Title;
        WreckMaterialId = image.WreckMaterialId;
    }


    public string? Title { get; set; } = string.Empty;
    public Guid? WreckMaterialId { get; set; }

    [Required]
    [DisplayName("Image Upload")]
    public IFormFile ImageFile { get; set; } 


    public Image ApplyChanges(Image image)
    {
        base.ApplyChanges(image);
        image.Title = Title;
        image.WreckMaterialId = WreckMaterialId;

        return image;
    }
}