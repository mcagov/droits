using System.Globalization;
using Droits.Models.Entities;
using Bogus;
using Droits.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace Droits.Data;

public static class DatabaseSeeder
{
    private static readonly Faker Faker;


    static DatabaseSeeder()
    {
        Faker = new Faker("en_GB");
    }


    public static void SeedData(DroitsContext dbContext)
    {
        dbContext.Database.EnsureCreated();

        if ( !dbContext.Users.Any() )
        {
            dbContext.Users.AddRange(GetUsers());
            dbContext.SaveChanges();
        }
        
        if ( !dbContext.Wrecks.Any() )
        {
            dbContext.Wrecks.AddRange(GetWrecks(Faker.Random.ArrayElement(dbContext.Users.ToArray())));
            dbContext.SaveChanges();
        }

        if ( !dbContext.Salvors.Any() )
        {
            dbContext.Salvors.AddRange(GetSalvors(Faker.Random.ArrayElement(dbContext.Users.ToArray())));
            dbContext.SaveChanges();
        }
        
        if ( !dbContext.Droits.Any() )
        {
            dbContext.Droits.AddRange(GetDroits(dbContext.Wrecks,
                dbContext.Salvors, dbContext.Users));
            dbContext.SaveChanges();
        }

        if ( !dbContext.Letters.Any() )
        {
            dbContext.Letters.AddRange(GetLetters(dbContext.Droits, Faker.Random.ArrayElement(dbContext.Users.ToArray())));
            dbContext.SaveChanges();
        }

        if ( !dbContext.Notes.Any() )
        {
            SeedNotes(dbContext);
            dbContext.SaveChanges();
        }
        
        if ( !dbContext.Images.Any() )
        {
            dbContext.Images.AddRange(GetImages());
            dbContext.SaveChanges();
        }

    }


    private static IEnumerable<Letter> GetLetters(IEnumerable<Droit> droits, ApplicationUser user)
    {
        return Enumerable.Range(0, 1)
            .Select(i => new Letter
            {
                Id = new Guid(),
                DroitId = Faker.Random.ArrayElement(droits.ToArray()).Id,
                Recipient = Faker.Internet.Email(),
                Subject = Faker.Lorem.Sentence(),
                Body = Faker.Lorem.Paragraph(),
                Type = Enum.GetValues(typeof(LetterType))
                    .OfType<LetterType>()
                    .MinBy(x => Guid.NewGuid()),
                SenderUserId = new Guid(),
                Created = DateTime.Now,
                LastModified = DateTime.Now,
                LastModifiedByUserId = user.Id,
            })
            .ToList();
    }


    private static IEnumerable<Salvor> GetSalvors(ApplicationUser user)
    {
        return Enumerable.Range(0, 3)
            .Select(i => new Salvor
            {
                Id = Guid.NewGuid(),
                Email = Faker.Internet.Email(),
                Name = Faker.Name.FullName(),
                TelephoneNumber = Faker.Phone.PhoneNumber(),
                Address = new Address
                {
                    Line1 = Faker.Address.StreetAddress(),
                    Line2 = Faker.Address.SecondaryAddress(),
                    Town = Faker.Address.City(),
                    County = Faker.Address.County(),
                    Postcode = Faker.Address.ZipCode()
                },
                DateOfBirth = Faker.Date.Past(40, DateTime.UtcNow),
                Created = DateTime.UtcNow,
                LastModified = DateTime.UtcNow,
                LastModifiedByUserId = user.Id,
            })
            .ToList();
    }


    private static IEnumerable<Droit> GetDroits(IEnumerable<Wreck> wrecks, IEnumerable<Salvor> salvors, IEnumerable<ApplicationUser> users)
    {
        return Enumerable.Range(0, 50)
            .Select(i => SeedDroit(Faker.Random.ArrayElement(wrecks.ToArray()),
                Faker.Random.ArrayElement(salvors.ToArray()),Faker.Random.ArrayElement(users.ToArray()) ))
            .ToList();
    }


    private static Droit SeedDroit(Wreck wreck, Salvor salvor, ApplicationUser user)
    {
        var reportedDate = Faker.Date.Past(3, DateTime.UtcNow);

        return new Droit
        {
            Id = Guid.NewGuid(),
            AssignedToUserId = user.Id,
            Reference = $"{Faker.Random.Int(0, 999):000}/" +
                        $"{reportedDate:yy}",
            Status = Enum.GetValues(typeof(DroitStatus))
                .OfType<DroitStatus>()
                .MinBy(x => Guid.NewGuid()),
            ReportedDate = reportedDate,
            DateFound = Faker.Date.Past(2, reportedDate),
            Created = DateTime.UtcNow,
            LastModified = DateTime.UtcNow,
            LastModifiedByUserId = user.Id,

            WreckId = wreck.Id,
            IsHazardousFind = Faker.Random.Bool(),
            IsDredge = Faker.Random.Bool(),

            SalvorId = salvor.Id,

            Latitude = wreck.Latitude,
            Longitude = wreck.Longitude,
            InUkWaters = Faker.Random.Bool(),
            LocationRadius = Faker.Random.Int(1, 500),
            Depth = Faker.Random.Int(1, 5000),
            LocationDescription = Faker.Lorem.Sentence(),

            SalvageAwardClaimed = Faker.Random.Bool(),
            ServicesDescription = Faker.Lorem.Sentence(),
            ServicesDuration = Faker.Lorem.Sentence(),
            ServicesEstimatedCost = Faker.Random.Int(1, 5000),
            MMOLicenceRequired = Faker.Random.Bool(),
            MMOLicenceProvided = Faker.Random.Bool(),
            SalvageClaimAwarded = Faker.Random.Float(),

            District = Faker.Address.County(),
            LegacyFileReference = Faker.Lorem.Sentence(),
            GoodsDischargedBy = Faker.Name.FullName(),
            DateDelivered = Faker.Date.Between(reportedDate, DateTime.UtcNow).ToShortDateString(),
            Agent = Faker.Name.FullName(),
            RecoveredFrom = Faker.Random.ArrayElement(new[] { "Afloat", "Ashore", "Seabed" }),
            ImportedFromLegacy = Faker.Random.Bool()
        };
    }


    private static IEnumerable<Wreck> GetWrecks(ApplicationUser user)
    {
        return Enumerable.Range(0, 50)
            .Select(i => new Wreck
            {
                Id = Guid.NewGuid(),
                Name = Faker.Name.FullName(),
                VesselConstructionDetails = Faker.Lorem.Sentence(),
                VesselYearConstructed = Faker.Random.Int(1500, DateTime.UtcNow.Year),
                DateOfLoss = Faker.Date.Past(500, DateTime.UtcNow),
                InUkWaters = Faker.Random.Bool(),
                IsWarWreck = Faker.Random.Bool(),
                IsAnAircraft = Faker.Random.Bool(),
                Latitude = Faker.Address.Latitude().ToString(CultureInfo.CurrentCulture),
                Longitude = Faker.Address.Longitude().ToString(CultureInfo.CurrentCulture),

                IsProtectedSite = Faker.Random.Bool(),
                ProtectionLegislation = Faker.Lorem.Sentence(),
                AdditionalInformation = Faker.Lorem.Sentence(),

                OwnerName = Faker.Name.FullName(),
                OwnerEmail = Faker.Internet.Email(),
                OwnerNumber = Faker.Phone.PhoneNumber(),

                Created = DateTime.UtcNow,
                LastModified = DateTime.UtcNow,
                LastModifiedByUserId = user.Id,
            })
            .ToList();
    }
    
    
    private static IEnumerable<ApplicationUser> GetUsers()
    {
        return Enumerable.Range(0, 5)
            .Select(i => new ApplicationUser()
            {
                Id = Guid.NewGuid(),
                AuthId = Guid.NewGuid().ToString(),
                Email = Faker.Internet.Email(),
                Name = Faker.Name.FullName(),
                Created = DateTime.UtcNow,
                LastModified = DateTime.UtcNow,
            })
            .ToList();
    }
    
    private static void SeedNotes(DroitsContext dbContext)
    {
        var notes = new List<Note>();

        notes.AddRange(GenerateNotesForEntities(dbContext, dbContext.Droits, nameof(Note.DroitId)));
        notes.AddRange(GenerateNotesForEntities(dbContext, dbContext.Wrecks, nameof(Note.WreckId)));
        notes.AddRange(GenerateNotesForEntities(dbContext, dbContext.Salvors, nameof(Note.SalvorId)));
        notes.AddRange(GenerateNotesForEntities(dbContext, dbContext.Letters, nameof(Note.LetterId)));
    
        dbContext.Notes.AddRange(notes);
    }


    private static IEnumerable<Note> GenerateNotesForEntities<T>(DroitsContext dbContext,
        IEnumerable<T> entities, string propertyName) where T : BaseEntity
    {
        var notes = new List<Note>();
        var randomUser = Faker.Random.ArrayElement(dbContext.Users.ToArray());

        foreach ( var entity in entities )
        {
            var numberOfNotes = Faker.Random.Int(1, 5);
            for ( var i = 0; i < numberOfNotes; i++ )
            {
                var note = new Note
                {
                    Text = Faker.Lorem.Paragraph(),
                    Type = NoteType.General,
                    Created = DateTime.UtcNow,
                    LastModified = DateTime.UtcNow,
                    LastModifiedByUserId = randomUser.Id,
                };

                typeof(Note).GetProperty(propertyName)?.SetValue(note, entity.Id);
                notes.Add(note);
            }
        }

        return notes;
    }


    private static IEnumerable<Image> GetImages()
    {
        return Enumerable.Range(0, 1)
            .Select(i => new Image()
            {
                Id = Guid.NewGuid(),
                Url="https://www.google.com/images/branding/googlelogo/2x/googlelogo_light_color_272x92dp.png",
                Created = DateTime.UtcNow,
                LastModified = DateTime.UtcNow,
            })
            .ToList();
    }
}
