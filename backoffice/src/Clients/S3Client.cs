using Amazon.S3;
using Amazon.S3.Model;

namespace Droits.Clients;

public interface IS3Client
{
    Task UploadImageAsync(string key, Stream imageStream, string contentType);
    Task<Stream> GetImageAsync(string key);
    Task DeleteImageAsync(string key);
}

public class S3Client : IS3Client
{
    private readonly IAmazonS3 _s3Client;
    private readonly string _bucketName;
    
    public S3Client(IAmazonS3 s3Client, IConfiguration configuration)
    {
        _s3Client = s3Client;
        _bucketName = configuration["S3:BucketName"];
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

        await _s3Client.PutObjectAsync(putRequest);
    }

    public async Task<Stream> GetImageAsync(string key)
    {
        var getRequest = new GetObjectRequest
        {
            BucketName = _bucketName,
            Key = key
        };

        var response = await _s3Client.GetObjectAsync(getRequest);
        return response.ResponseStream;
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