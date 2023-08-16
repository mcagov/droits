using Droits.Models.Entities;

namespace Droits.Models.FormModels;

public class ImageForm : BaseEntityForm
{
    public ImageForm()
    {
        
    }


    public ImageForm(Image image)
    {
        Url = image.Url;
    }
    
    public string Url { get; set; }


    public Image ApplyChanges(Image image)
    {
        image.Url = Url;

        return image;
    }
}