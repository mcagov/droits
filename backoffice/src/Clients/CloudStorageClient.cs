
#region

using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.IdentityModel.Tokens;

#endregion

namespace Droits.Clients;

public interface ICloudStorageClient
{
    Task UploadFileAsync(string? key, Stream imageStream, string contentType);
    Task<Stream> GetFileAsync(string? key);
    Task DeleteFileAsync(string? key);
}

public class CloudStorageClient : ICloudStorageClient
{
    private readonly IAmazonS3 _s3Client;
    private readonly ILogger<CloudStorageClient> _logger;

    private readonly string? _bucketName;
    
    public CloudStorageClient(IAmazonS3 s3Client, ILogger<CloudStorageClient> logger, IConfiguration configuration)
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

    public async Task UploadFileAsync(string? key, Stream imageStream, string contentType)
    {
        if ( string.IsNullOrEmpty(key) )
        {
            return;
        }
        
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

    public async Task<Stream> GetFileAsync(string? key)
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
    
    public async Task DeleteFileAsync(string? key)
    {
        if ( string.IsNullOrEmpty(key) )
        {
            return;
        }
        var deleteRequest = new DeleteObjectRequest
        {
            BucketName = _bucketName,
            Key = key
        };

        await _s3Client.DeleteObjectAsync(deleteRequest);
    }
}