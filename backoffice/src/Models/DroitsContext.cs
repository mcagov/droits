using Droits.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Droits.Models;

public partial class DroitsContext : DbContext
{
    public DroitsContext()
    {
    }

    public DroitsContext(DbContextOptions<DroitsContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Droit> Droits { get; set; }
    public virtual DbSet<Wreck> Wrecks { get; set; }
    public virtual DbSet<Email> Emails { get; set; }
    public virtual DbSet<Salvor> Salvors { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {


        modelBuilder.Entity<Droit>(entity =>
        {
            entity.ToTable("droits");

            entity.Property(e => e.Id);
            entity.Property(e => e.Status);
            entity.Property(e => e.ReportedDate);
            entity.Property(e => e.Created);
            entity.Property(e => e.LastModified);
            entity.Property(e => e.Reference);
            entity.Property(e => e.IsHazardousFind);

            //Wreck
            entity.Property(e => e.WreckConstructionDetails);
            entity.Property(e => e.WreckVesselName);
            entity.Property(e => e.WreckVesselYearConstructed);
            entity.Property(e => e.WreckVesselYearSunk);

            // Location
            entity.Property(e => e.Latitude);
            entity.Property(e => e.Longitude);
            entity.Property(e => e.InUkWaters);
            entity.Property(e => e.LocationRadius);
            entity.Property(e => e.Depth);
            entity.Property(e => e.LocationDescription);

            // Salvage

            entity.Property(e => e.SalvageAwardClaimed);
            entity.Property(e => e.ServicesDescription);
            entity.Property(e => e.ServicesDuration);
            entity.Property(e => e.ServicesEstimatedCost);
            entity.Property(e => e.MMOLicenceRequired);
            entity.Property(e => e.MMOLicenceProvided);
            entity.Property(e => e.SalvageClaimAwarded);

            // Legacy fields

            entity.Property(e => e.District);
            entity.Property(e => e.LegacyFileReference);
            entity.Property(e => e.GoodsDischargedBy);
            entity.Property(e => e.DateDelivered);
            entity.Property(e => e.Agent);
            entity.Property(e => e.RecoveredFrom);
            entity.Property(e => e.ImportedFromLegacy);
        });


        modelBuilder.Entity<Wreck>(entity =>
        {
            entity.ToTable("wrecks");

            entity.Property(e => e.Id);
            entity.Property(e => e.Status);
            entity.Property(e => e.Name);
            entity.Property(e => e.DateOfLoss);
            entity.Property(e => e.IsWarWreck);
            entity.Property(e => e.IsAnAircraft);
            entity.Property(e => e.Latitude);
            entity.Property(e => e.Longitude);
            entity.Property(e => e.IsProtectedSite);
            entity.Property(e => e.ProtectionLegislation);
            entity.Property(e => e.Created);
            entity.Property(e => e.LastModified);

        });

        modelBuilder.Entity<Email>(entity =>
        {
            entity.ToTable("emails");

            entity.Property(e => e.Id);
            entity.Property(e => e.Subject);
            entity.Property(e => e.Body);
            entity.Property(e => e.DateSent);
            entity.Property(e => e.Recipient);
            entity.Property(e => e.Created);
            entity.Property(e => e.LastModified);
            entity.Property(e => e.Type);
        });
        
        modelBuilder.Entity<Salvor>(entity =>
        {
            entity.ToTable("salvors");

            entity.Property(e => e.Id);
            entity.Property(e => e.Email);
            entity.Property(e => e.Name);
            entity.Property(e => e.TelephoneNumber);
            entity.Property(e => e.DateOfBirth);
            entity.Property(e => e.Created);
            entity.Property(e => e.LastModified);
            entity.OwnsOne(e => e.Address);

        });

        base.OnModelCreating(modelBuilder);

    }




    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
