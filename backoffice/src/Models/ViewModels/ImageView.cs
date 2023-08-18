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
        Title = image.Title;
        UploadDate = image.Created;
    }
    
    public Guid Id { get; }
    public string Title { get; }
    public DateTime UploadDate { get; }

}