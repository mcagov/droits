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
        Key = image.Key;
    }
    
    public Guid Id { get; }
    public string Key { get; }
}