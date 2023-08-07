using Droits.Models.Entities;
using Bogus;
using Droits.Models.Enums;

namespace Droits.Data;

public static class DatabaseSeeder
{
    private static Faker _faker;


    static DatabaseSeeder()
    {
        _faker = new Faker("en_GB");
    }


    public static void SeedData(DroitsContext dbContext)
    {
        dbContext.Database.EnsureCreated();

        if ( !dbContext.Wrecks.Any() )
        {
            dbContext.Wrecks.AddRange(GetWrecks());
            dbContext.SaveChanges();
        }

        if ( !dbContext.Salvors.Any() )
        {
            dbContext.Salvors.AddRange(GetSalvors());
            dbContext.SaveChanges();
        }

        if ( !dbContext.Droits.Any() )
        {
            dbContext.Droits.AddRange(GetDroits(dbContext.Wrecks.ToList(),
                dbContext.Salvors.ToList()));
            dbContext.SaveChanges();
        }

        if ( !dbContext.Letters.Any() )
        {
            dbContext.Letters.AddRange(GetLetters(dbContext.Droits.ToList()));
            dbContext.SaveChanges();
        }
    }


    private static List<Letter> GetLetters(List<Droit> droits)
    {
        return Enumerable.Range(0, 2)
            .Select(i => new Letter
            {
                Id = new Guid(),
                DroitId = _faker.Random.ArrayElement(droits.ToArray()).Id,
                Recipient = _faker.Internet.Email(),
                Subject = _faker.Lorem.Sentence(),
                Body = _faker.Lorem.Paragraph(),
                Type = Enum.GetValues(typeof(LetterType))
                    .OfType<LetterType>()
                    .MinBy(x => Guid.NewGuid()),
                SenderUserId = new Guid(),
                Created = DateTime.Now,
                LastModified = DateTime.Now
            })
            .ToList();
    }


    private static List<Salvor> GetSalvors()
    {
        return Enumerable.Range(0, 150)
            .Select(i => new Salvor
            {
                Id = Guid.NewGuid(),
                Email = _faker.Internet.Email(),
                Name = _faker.Name.FullName(),
                TelephoneNumber = _faker.Phone.PhoneNumber(),
                Address = new Address
                {
                    Line1 = _faker.Address.StreetAddress(),
                    Line2 = _faker.Address.SecondaryAddress(),
                    Town = _faker.Address.City(),
                    County = _faker.Address.County(),
                    Postcode = _faker.Address.ZipCode()
                },
                DateOfBirth = _faker.Date.Past(40, DateTime.UtcNow),
                Created = DateTime.Now,
                LastModified = DateTime.Now
            })
            .ToList();
    }


    private static List<Droit> GetDroits(List<Wreck> wrecks, List<Salvor> salvors)
    {
        return Enumerable.Range(0, 50)
            .Select(i => SeedDroit(_faker.Random.ArrayElement(wrecks.ToArray()),
                _faker.Random.ArrayElement(salvors.ToArray())))
            .ToList();
    }


    private static Droit SeedDroit(Wreck wreck, Salvor salvor)
    {
        var reportedDate = _faker.Date.Past(3, DateTime.UtcNow);

        return new Droit
        {
            Id = Guid.NewGuid(),
            Reference = $"{_faker.Random.Int(0, 999).ToString("000")}/" +
                        $"{reportedDate.ToString("yy")}",
            Status = Enum.GetValues(typeof(DroitStatus))
                .OfType<DroitStatus>()
                .MinBy(x => Guid.NewGuid()),
            ReportedDate = reportedDate,
            DateFound = _faker.Date.Past(2, reportedDate),
            Created = DateTime.UtcNow,
            LastModified = DateTime.UtcNow,

            WreckId = wreck.Id,
            IsHazardousFind = _faker.Random.Bool(),
            IsDredge = _faker.Random.Bool(),

            SalvorId = salvor.Id,

            Latitude = wreck.Latitude,
            Longitude = wreck.Longitude,
            InUkWaters = _faker.Random.Bool(),
            LocationRadius = _faker.Random.Int(1, 500),
            Depth = _faker.Random.Int(1, 5000),
            LocationDescription = _faker.Lorem.Sentence(),

            SalvageAwardClaimed = _faker.Random.Bool(),
            ServicesDescription = _faker.Lorem.Sentence(),
            ServicesDuration = _faker.Lorem.Sentence(),
            ServicesEstimatedCost = _faker.Random.Int(1, 5000),
            MMOLicenceRequired = _faker.Random.Bool(),
            MMOLicenceProvided = _faker.Random.Bool(),
            SalvageClaimAwarded = _faker.Random.Float(),

            District = _faker.Address.County(),
            LegacyFileReference = _faker.Lorem.Sentence(),
            GoodsDischargedBy = _faker.Name.FullName(),
            DateDelivered = _faker.Date.Between(reportedDate, DateTime.UtcNow).ToShortDateString(),
            Agent = _faker.Name.FullName(),
            RecoveredFrom = _faker.Random.ArrayElement(new[] { "Afloat", "Ashore", "Seabed" }),
            ImportedFromLegacy = _faker.Random.Bool()
        };
    }


    private static List<Wreck> GetWrecks()
    {
        return Enumerable.Range(0, 50)
            .Select(i => new Wreck
            {
                Id = Guid.NewGuid(),
                Name = _faker.Name.FullName(),
                VesselConstructionDetails = _faker.Lorem.Sentence(),
                VesselYearConstructed = _faker.Random.Int(1500, DateTime.UtcNow.Year),
                DateOfLoss = _faker.Date.Past(500, DateTime.UtcNow),
                InUkWaters = _faker.Random.Bool(),
                IsWarWreck = _faker.Random.Bool(),
                IsAnAircraft = _faker.Random.Bool(),
                Latitude = _faker.Address.Latitude().ToString(),
                Longitude = _faker.Address.Longitude().ToString(),

                IsProtectedSite = _faker.Random.Bool(),
                ProtectionLegislation = _faker.Lorem.Sentence(),
                AdditionalInformation = _faker.Lorem.Sentence(),

                OwnerName = _faker.Name.FullName(),
                OwnerEmail = _faker.Internet.Email(),
                OwnerNumber = _faker.Phone.PhoneNumber(),

                Created = DateTime.UtcNow,
                LastModified = DateTime.UtcNow
            })
            .ToList();
    }
}
