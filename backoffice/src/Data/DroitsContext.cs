using Droits.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Droits.Data;

public partial class DroitsContext : DbContext
{
    public DroitsContext()
    {
    }


    public DroitsContext(DbContextOptions<DroitsContext> options)
        : base(options)
    {
    }


    public virtual DbSet<Droit> Droits { get; set; } = null!;
    public virtual DbSet<WreckMaterial> WreckMaterials { get; set; } = null!;
    public virtual DbSet<Wreck> Wrecks { get; set; } = null!;
    public virtual DbSet<Letter> Letters { get; set; } = null!;
    public virtual DbSet<Salvor> Salvors { get; set; } = null!;


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
    }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Droit>(entity =>
        {
            entity.ToTable("droits");

            entity.Property(d => d.Id);
            entity.Property(d => d.Status);
            entity.Property(d => d.ReportedDate);
            entity.Property(d => d.DateFound);
            entity.Property(d => d.Created);
            entity.Property(d => d.LastModified);
            entity.Property(d => d.Reference);
            entity.Property(d => d.IsHazardousFind);
            entity.Property(d => d.IsDredge);

            // Location
            entity.Property(d => d.Latitude);
            entity.Property(d => d.Longitude);
            entity.Property(d => d.InUkWaters);
            entity.Property(d => d.LocationRadius);
            entity.Property(d => d.Depth);
            entity.Property(d => d.LocationDescription);

            // Salvage

            entity.Property(d => d.SalvageAwardClaimed);
            entity.Property(d => d.ServicesDescription);
            entity.Property(d => d.ServicesDuration);
            entity.Property(d => d.ServicesEstimatedCost);
            entity.Property(d => d.MMOLicenceRequired);
            entity.Property(d => d.MMOLicenceProvided);
            entity.Property(d => d.SalvageClaimAwarded);

            // Legacy fields

            entity.Property(d => d.District);
            entity.Property(d => d.LegacyFileReference);
            entity.Property(d => d.GoodsDischargedBy);
            entity.Property(d => d.DateDelivered);
            entity.Property(d => d.Agent);
            entity.Property(d => d.RecoveredFrom);
            entity.Property(d => d.ImportedFromLegacy);

            //Relationships

            entity.HasOne(d => d.Wreck)
                .WithMany(w => w.Droits)
                .HasForeignKey(d => d.WreckId)
                .IsRequired(false);
            entity.HasOne(d => d.Salvor)
                .WithMany(s => s.Droits)
                .HasForeignKey(d => d.SalvorId)
                .IsRequired(false);
        });


        modelBuilder.Entity<WreckMaterial>(entity =>
        {
            entity.ToTable("wreck_materials");

            entity.Property(w => w.Id);
            entity.Property(w => w.DroitId);
            entity.Property(w => w.Name);
            entity.Property(w => w.Description);
            entity.Property(w => w.Quantity);
            entity.Property(w => w.Value);
            entity.Property(w => w.ReceiverValuation);
            entity.Property(w => w.ValueConfirmed);
            // entity.Property(w => w.Images);
            entity.Property(w => w.Created);
            entity.Property(w => w.LastModified);
            entity.Property(w => w.WreckMaterialOwner);
            entity.Property(w => w.Purchaser);
            entity.Property(w => w.Outcome);
            entity.Property(w => w.WhereSecured);

            entity.OwnsOne(w => w.StorageAddress);

            entity.HasOne(w => w.Droit)
                .WithMany(d => d.WreckMaterials)
                .HasForeignKey(w => w.DroitId)
                .IsRequired();
        });


        modelBuilder.Entity<Wreck>(entity =>
        {
            entity.ToTable("wrecks");

            entity.Property(w => w.Id);
            entity.Property(w => w.Name);
            entity.Property(w => w.VesselYearConstructed);
            entity.Property(w => w.VesselConstructionDetails);
            entity.Property(w => w.DateOfLoss);
            entity.Property(w => w.InUkWaters);
            entity.Property(w => w.IsWarWreck);
            entity.Property(w => w.IsAnAircraft);
            entity.Property(w => w.Latitude);
            entity.Property(w => w.Longitude);
            entity.Property(w => w.IsProtectedSite);
            entity.Property(w => w.ProtectionLegislation);
            entity.Property(w => w.OwnerName);
            entity.Property(w => w.OwnerEmail);
            entity.Property(w => w.OwnerNumber);
            entity.Property(w => w.Created);
            entity.Property(w => w.LastModified);
            entity.Property(w => w.AdditionalInformation);
            entity.HasMany(w => w.Droits)
                .WithOne(d => d.Wreck)
                .HasForeignKey(d => d.WreckId);
        });

        modelBuilder.Entity<Letter>(entity =>
        {
            entity.ToTable("letters");

            entity.Property(l => l.Id);
            entity.Property(l => l.Subject);
            entity.Property(l => l.Body);
            entity.Property(l => l.DateSent);
            entity.Property(l => l.Recipient);
            entity.Property(l => l.Created);
            entity.Property(l => l.LastModified);
            entity.Property(l => l.Type);
            entity.HasOne(l => l.Droit)
                .WithMany(d => d.Letters)
                .HasForeignKey(l => l.DroitId)
                .IsRequired(false);
        });

        modelBuilder.Entity<Salvor>(entity =>
        {
            entity.ToTable("salvors");

            entity.Property(s => s.Id);
            entity.Property(s => s.Email);
            entity.Property(s => s.Name);
            entity.Property(s => s.TelephoneNumber);
            entity.Property(s => s.DateOfBirth);
            entity.Property(s => s.Created);
            entity.Property(s => s.LastModified);
            entity.OwnsOne(s => s.Address);
            entity.HasMany(s => s.Droits)
                .WithOne(d => d.Salvor)
                .HasForeignKey(d => d.SalvorId);
        });

        base.OnModelCreating(modelBuilder);
    }


    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
