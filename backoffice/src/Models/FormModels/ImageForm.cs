using Droits.Models.Entities;

namespace Droits.Models.FormModels;

public class ImageForm : BaseEntityForm
{
    public ImageForm()
    {
        
    }


    public ImageForm(Image image) : base(image)
    {
        Url = image.Url;
    }
    
    public string Url { get; set; }
    public Guid? WreckMaterialId { get; set; }


    public Image ApplyChanges(Image image)
    {
        base.ApplyChanges(image);
        image.Url = Url;
        image.WreckMaterialId = WreckMaterialId;

        return image;
    }
}