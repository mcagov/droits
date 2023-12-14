
#region

using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.IdentityModel.Tokens;

#endregion

namespace Droits.Clients;

public interface IImageStorageClient
{
    Task UploadImageAsync(string key, Stream imageStream, string contentType);
    Task<Stream> GetImageAsync(string key);
    Task DeleteImageAsync(string key);
}

public class ImageStorageClient : IImageStorageClient
{
    private readonly IAmazonS3 _s3Client;
    private readonly ILogger<ImageStorageClient> _logger;

    private readonly string? _bucketName;
    
    public ImageStorageClient(IAmazonS3 s3Client, ILogger<ImageStorageClient> logger, IConfiguration configuration)
    {
        _s3Client = s3Client;
        _logger = logger;
        _bucketName = configuration["AWS:S3Config:BucketName"];

        if ( _bucketName.IsNullOrEmpty() )
        {
            _logger.LogError("Bucket Name cannot be null");
            throw new Exception("Bucket Name cannot be null");
        }
    }

    public async Task UploadImageAsync(string key, Stream imageStream, string contentType)
    {
        var putRequest = new PutObjectRequest
        {
            BucketName = _bucketName,
            Key = key,
            ContentType = contentType,
            InputStream = imageStream
        };
       
        try
        {
            await _s3Client.PutObjectAsync(putRequest);
        }
        catch ( AmazonS3Exception e )
        {
            
            _logger.LogError($"AWS S3 Exception Putting Image - {e.Message} , {e.StackTrace}");
            throw;
        }
        catch ( Exception e )
        {
            _logger.LogError($"Exception Putting Image - {e.Message} , {e.StackTrace}");
            throw;
        }
        
    }

    public async Task<Stream> GetImageAsync(string key)
    {
        
        var getRequest = new GetObjectRequest
        {
            BucketName = _bucketName,
            Key = key
        };

        try
        {
            var response = await _s3Client.GetObjectAsync(getRequest);
            return response.ResponseStream;
        }
        catch ( Exception e )
        {
            _logger.LogError($"Exception getting image - {e.Message} , {e.StackTrace}");
            throw;
        }
    }
    
    public async Task DeleteImageAsync(string key)
    {
        var deleteRequest = new DeleteObjectRequest
        {
            BucketName = _bucketName,
            Key = key
        };

        await _s3Client.DeleteObjectAsync(deleteRequest);
    }
}