using Amazon.S3;
using Droits.Data;
using Droits.Local;
using Droits.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Notify.Client;

namespace Droits.Tests.UnitTests.Local;

public class LocalServicesTests
{
    private static DroitsContext CreateDbContext() =>
        new(new DbContextOptionsBuilder<DroitsContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options);

    [Fact]
    public void AddFallbackConfiguration_FillsInMissingGovNotifyAndBlobSettings()
    {
        var configuration = new ConfigurationManager();

        LocalServices.AddFallbackConfiguration(configuration);

        Assert.Equal(LocalServices.PlaceholderGovNotifyKey, configuration["GovNotify:ApiKey"]);
        Assert.Equal("UseDevelopmentStorage=true", configuration["Azure:BlobConfig:ConnectionString"]);
    }

    [Fact]
    public void AddFallbackConfiguration_KeepsConfiguredSettings()
    {
        var configuration = new ConfigurationManager();
        configuration.AddInMemoryCollection(new Dictionary<string, string?>
        {
            ["GovNotify:ApiKey"] = "real-key",
            ["Azure:BlobConfig:ConnectionString"] = "real-connection"
        });

        LocalServices.AddFallbackConfiguration(configuration);

        Assert.Equal("real-key", configuration["GovNotify:ApiKey"]);
        Assert.Equal("real-connection", configuration["Azure:BlobConfig:ConnectionString"]);
    }

    [Fact]
    public void PlaceholderGovNotifyKey_IsAcceptedByTheNotifyClient()
    {
        var client = new NotificationClient(LocalServices.PlaceholderGovNotifyKey);

        Assert.NotNull(client);
    }

    [Fact]
    public void CreateS3Client_PointsAtLocalStackWithPathStyleUrls()
    {
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?> { ["AWS:ServiceURL"] = "http://localhost:4566" })
            .Build();

        var config = (AmazonS3Config)LocalServices.CreateS3Client(configuration).Config;

        Assert.Equal("http://localhost:4566/", config.ServiceURL);
        Assert.True(config.ForcePathStyle);
    }

    [Fact]
    public void SeedSalvor_AddsTheLocalSalvorWithThreeDroits()
    {
        using var dbContext = CreateDbContext();
        dbContext.Droits.AddRange(Enumerable.Range(1, 5).Select(i => new Droit { Id = Guid.NewGuid(), Reference = $"00{i}/26" }));
        dbContext.SaveChanges();

        LocalServices.SeedSalvor(dbContext, "dev@droits.local");

        var salvor = dbContext.Salvors.Single(s => s.Email == "dev@droits.local");
        Assert.Equal(3, dbContext.Droits.Count(d => d.SalvorId == salvor.Id));
    }

    [Fact]
    public void SeedSalvor_DoesNothingWhenTheSalvorExists()
    {
        using var dbContext = CreateDbContext();
        dbContext.Salvors.Add(new Salvor { Id = Guid.NewGuid(), Email = "dev@droits.local" });
        dbContext.SaveChanges();

        LocalServices.SeedSalvor(dbContext, "dev@droits.local");

        Assert.Single(dbContext.Salvors);
    }
}
