using Amazon.S3;
using Amazon.S3.Model;

namespace Droits.Clients;

public interface IS3Client
{
    Task UploadImageAsync(string key, Stream imageStream);
    Task<Stream> GetImageAsync(string key);
    Task<String> GetImageTypeAsync(string key);
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

    public async Task UploadImageAsync(string key, Stream imageStream)
    {
        var putRequest = new PutObjectRequest
        {
            BucketName = _bucketName,
            Key = key,
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
    public async Task<String> GetImageTypeAsync(string key)
    {
        var getRequest = new GetObjectRequest
        {
            BucketName = _bucketName,
            Key = key
        };

        var response = await _s3Client.GetObjectAsync(getRequest);
        return response.Headers.ContentType;
    }
}