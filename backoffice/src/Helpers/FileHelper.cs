using Droits.Models.Entities;
using Microsoft.AspNetCore.StaticFiles;

namespace Droits.Helpers;

public static class FileHelper
{
    private static readonly FileExtensionContentTypeProvider ContentTypeProvider = new ();
    private static readonly string[] BrowserMimeTypes = new[]
    {
        "application/pdf",
        "image/jpeg", "image/png", "image/gif", "image/bmp", "image/svg+xml",
        "text/plain", "text/html", "text/css", "application/javascript",
        "audio/mpeg", "audio/mp4", "video/webm", "audio/ogg",
        "application/msword", "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
        "application/vnd.ms-excel", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
        "application/vnd.ms-powerpoint", "application/vnd.openxmlformats-officedocument.presentationml.presentation",
        "application/json"
    };
    
    private static readonly string[] ImageExtensions= new[] { ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".svg" , ".webp"};

    public static string GetContentType(string fileName)
    {
        var fileExtension = Path.GetExtension(fileName);

        return ContentTypeProvider.TryGetContentType(fileExtension, out var contentType) ? contentType : "application/octet-stream";
    }
    public static bool CanOpen(string filename)
    {
        var contentType = GetContentType(filename);

        return BrowserMimeTypes.Contains(contentType);
    }
    
    public static bool IsImage(string filename)
    {
        var extension = Path.GetExtension(filename);

        return ImageExtensions.Contains(extension, StringComparer.OrdinalIgnoreCase);
    }
    
    public static string? GetFileExtension(string mimeType)
    {
        var fileExtension = ContentTypeProvider.Mappings
            .FirstOrDefault(pair => string.Equals(pair.Value, mimeType, StringComparison.OrdinalIgnoreCase))
            .Key;

        return fileExtension;
    }
    
    public static string? GetFileKey(DroitFile droitFile, string? filename)
    {
        if ( droitFile.WreckMaterial != null )
        {
            return  $"Droits/{droitFile.WreckMaterial.DroitId}/WreckMaterials/{droitFile.WreckMaterialId}/DroitFiles/{droitFile.Id}_{filename}";

        }

        if ( droitFile.Note == null ) return null;
        
        var note = droitFile.Note;

        var basePath = note switch
        {
            { DroitId: not null } => $"Droits/{note.DroitId}",
            { WreckId: not null } => $"Wrecks/{note.WreckId}",
            { SalvorId: not null } => $"Salvors/{note.SalvorId}",
            { LetterId: not null } => $"Letters/{note.LetterId}",

            _ => throw new InvalidOperationException("Unknown note condition")
        };

        return $"{basePath}/Notes/{note.Id}/DroitFiles/{droitFile.Id}_{filename}";

    }
}