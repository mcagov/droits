#region

using Droits.Models.Entities;

#endregion

namespace Droits.Models.ViewModels;

public class DroitFileView : BaseEntityView
{
    public DroitFileView()
    {
        
    }


    public DroitFileView(DroitFile droitFile) : base(droitFile)
    {
        Id = droitFile.Id;
        Title = droitFile.Title ?? "File";
        Url = droitFile.Url;
        Filename = droitFile.Filename;
        UploadDate = droitFile.Created;
    }
    
    public Guid Id { get; }
    public string Title { get; } = string.Empty;
    public string? Url { get; } = string.Empty;
    public string? Filename { get; } = string.Empty;

    public DateTime UploadDate { get; }

}