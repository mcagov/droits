using Amazon.Runtime;
using Amazon.S3;
using Droits.Data;
using Droits.Models.Entities;

namespace Droits.Local;

public static class LocalServices
{
    public const string PlaceholderGovNotifyKey =
        "local-00000000-0000-4000-8000-000000000000-00000000-0000-4000-8000-000000000000";

    public static void AddFallbackConfiguration(IConfigurationManager configuration)
    {
        var fallbacks = new Dictionary<string, string?>();

        if ( string.IsNullOrEmpty(configuration["GovNotify:ApiKey"]) )
        {
            fallbacks["GovNotify:ApiKey"] = PlaceholderGovNotifyKey;
        }

        if ( string.IsNullOrEmpty(configuration["Azure:BlobConfig:ConnectionString"]) )
        {
            fallbacks["Azure:BlobConfig:ConnectionString"] = "UseDevelopmentStorage=true";
        }

        configuration.AddInMemoryCollection(fallbacks);
    }

    public static IAmazonS3 CreateS3Client(IConfiguration configuration) =>
        new AmazonS3Client(
            new BasicAWSCredentials("test", "test"),
            new AmazonS3Config
            {
                ServiceURL = configuration["AWS:ServiceURL"] ?? "http://localhost:4566",
                AuthenticationRegion = configuration["AWS:Region"] ?? "eu-west-2",
                ForcePathStyle = true
            });

    public static void SeedSalvor(DroitsContext dbContext, string email)
    {
        if ( dbContext.Salvors.Any(s => s.Email == email) )
        {
            return;
        }

        var salvor = new Salvor
        {
            Id = Guid.NewGuid(),
            Email = email,
            Name = "Dev User",
            Created = DateTime.UtcNow,
            LastModified = DateTime.UtcNow
        };
        dbContext.Salvors.Add(salvor);

        foreach ( var droit in dbContext.Droits.OrderBy(d => d.Reference).Take(3) )
        {
            droit.SalvorId = salvor.Id;
        }

        dbContext.SaveChanges();
    }
}
