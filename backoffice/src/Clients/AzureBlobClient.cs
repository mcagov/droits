using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace Droits.Clients;
public interface IAzureBlobClient
{
    Task<Stream> GetAzureImageStreamAsync(string imageUrl);
}

public class AzureBlobClient : IAzureBlobClient
{
    private readonly BlobServiceClient? _blobServiceClient;

    public AzureBlobClient(IConfiguration? configuration)
    {
        if ( configuration == null ) return;
        var connectionString = configuration["Azure:BlobConfig:ConnectionString"];
        if (string.IsNullOrEmpty(connectionString))
        {
            throw new ArgumentException("Azure Blob Storage connection string is missing or empty.");
        }

        _blobServiceClient = new BlobServiceClient(connectionString);
    }

    public async Task<Stream> GetAzureImageStreamAsync(string imageUrl)
    {
        try
        {
            var uri = new Uri(imageUrl);
            var containerName = uri.Segments[1].TrimEnd('/');
            var blobName = Uri.UnescapeDataString(uri.Segments[2]);

            var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            var blobClient = containerClient.GetBlobClient(blobName);

            BlobDownloadInfo download = await blobClient.DownloadAsync();
            return download.Content;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error getting Azure image: {ex.Message}");
            throw;
        }
    }
}