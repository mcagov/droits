#region

using Droits.Models.Entities;

#endregion

namespace Droits.Models.ViewModels;

public class ImageView : BaseEntityView
{
    public ImageView()
    {
        
    }


    public ImageView(Image image) : base(image)
    {
        Id = image.Id;
        Title = image.Title ?? "Image";
        UploadDate = image.Created;
    }
    
    public Guid Id { get; }
    public string Title { get; } = string.Empty;
    public DateTime UploadDate { get; }

}