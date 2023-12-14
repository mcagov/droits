using Microsoft.AspNetCore.StaticFiles;

namespace Droits.Helpers;

public static class FileHelper
{
    private static readonly FileExtensionContentTypeProvider ContentTypeProvider = new ();
    public static string GetContentType(string fileName)
    {
        var fileExtension = Path.GetExtension(fileName);

        return ContentTypeProvider.TryGetContentType(fileExtension, out var contentType) ? contentType : "application/octet-stream";
    }
}