namespace Droits.Models.Entities;
using Droits.Models;

public static class DatabaseSeeder
{
    public static void SeedData(DroitsContext dbContext)
    {
        dbContext.Database.EnsureCreated();

        if (!dbContext.Wrecks.Any())
        {
            dbContext.Wrecks.AddRange(GetWrecks());
        }

        dbContext.SaveChanges();
    }

    private static List<Wreck> GetWrecks() => new List<Wreck>{
            new Wreck
            {
                Id = Guid.NewGuid(),
                Name = "Great Wall of China",
                Latitude = "40.431908",
                Longitude = "116.570374",
                Created = DateTime.UtcNow,
                LastModified = DateTime.UtcNow
            },
            new Wreck
            {
                Id = Guid.NewGuid(),
                Name = "Petra",
                Latitude = "30.328611",
                Longitude = "35.444445",
                Created = DateTime.UtcNow,
                LastModified = DateTime.UtcNow
            },
            new Wreck
            {
                Id = Guid.NewGuid(),
                Name = "Christ the Redeemer",
                Latitude = "-22.951944",
                Longitude = "-43.210556",
                Created = DateTime.UtcNow,
                LastModified = DateTime.UtcNow
            },
            new Wreck
            {
                Id = Guid.NewGuid(),
                Name = "Machu Picchu",
                Latitude = "-13.163056",
                Longitude = "-72.545556",
                Created = DateTime.UtcNow,
                LastModified = DateTime.UtcNow
            },
            new Wreck
            {
                Id = Guid.NewGuid(),
                Name = "Chichen Itza",
                Latitude = "20.682778",
                Longitude = "-88.569167",
                Created = DateTime.UtcNow,
                LastModified = DateTime.UtcNow
            },
            new Wreck
            {
                Id = Guid.NewGuid(),
                Name = "Colosseum",
                Latitude = "41.890169",
                Longitude = "12.492269",
                Created = DateTime.UtcNow,
                LastModified = DateTime.UtcNow
            },
            new Wreck
            {
                Id = Guid.NewGuid(),
                Name = "Taj Mahal",
                Latitude = "27.175015",
                Longitude = "78.042155",
                Created = DateTime.UtcNow,
                LastModified = DateTime.UtcNow
            }
        };
}
