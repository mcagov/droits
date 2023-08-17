using Droits.Models.Entities;

namespace Droits.Models.ViewModels;

public class ImageView : BaseEntityView
{
    public ImageView()
    {
        
    }


    public ImageView(Image image) : base(image)
    {
        Id = image.Id;
        Url = image.Url;
    }
    
    public Guid Id { get; }
    public string Url { get; }
}