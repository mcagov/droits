#region

using Droits.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

#endregion

namespace Droits.Data;

public partial class DroitsContext : DbContext
{
    public DroitsContext()
    {
         AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
    }


    public DroitsContext(DbContextOptions<DroitsContext> options)
        : base(options)
    {
         AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
    }

    public virtual DbSet<ApplicationUser> Users { get; set; } = null!;
    public virtual DbSet<Note> Notes { get; set; } = null!;
    public virtual DbSet<Droit> Droits { get; set; } = null!;
    public virtual DbSet<WreckMaterial> WreckMaterials { get; set; } = null!;
    public virtual DbSet<Wreck> Wrecks { get; set; } = null!;
    public virtual DbSet<Letter> Letters { get; set; } = null!;
    public virtual DbSet<Salvor> Salvors { get; set; } = null!;
    public virtual DbSet<Image> Images { get; set; } = null!;
    public virtual DbSet<DroitFile> DroitFiles { get; set; } = null!;


    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
    }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        
        modelBuilder.HasPostgresExtension("fuzzystrmatch");
                
        modelBuilder.Entity<ApplicationUser>(entity =>
        {
            entity.ToTable("users");
            entity.Property(u => u.Id);
            entity.Property(u => u.AuthId);
            entity.Property(u => u.Name);
            entity.Property(u => u.Email);
            entity.Property(u => u.Created);
            entity.Property(u => u.LastModified);
        });
        
        modelBuilder.Entity<Note>(entity =>
        {
            entity.ToTable("notes");

            entity.Property(n => n.Id);
            entity.Property(n => n.Title);
            entity.Property(n => n.Text);
            entity.Property(n => n.Type);
            entity.Property(n => n.Created);
            entity.Property(n => n.LastModified);
            entity.Property(n => n.LastModifiedByUserId).IsRequired(false);

            // Relationships
            entity.HasOne(n => n.Droit)
                .WithMany(d => d.Notes)
                .HasForeignKey(n => n.DroitId)
                .IsRequired(false);

            entity.HasOne(n => n.Wreck)
                .WithMany(w => w.Notes)
                .HasForeignKey(n => n.WreckId)
                .IsRequired(false);

            entity.HasOne(n => n.Salvor)
                .WithMany(s => s.Notes)
                .HasForeignKey(n => n.SalvorId)
                .IsRequired(false);
            
            entity.HasOne(n => n.Letter)
                .WithMany(s => s.Notes)
                .HasForeignKey(n => n.LetterId)
                .IsRequired(false);

            entity.HasOne(n => n.LastModifiedByUser)
                .WithMany()
                .HasForeignKey(n => n.LastModifiedByUserId)
                .IsRequired(false);
            
            entity.HasMany(n => n.Files)
                .WithOne(i => i.Note)
                .HasForeignKey(i => i.NoteId)
                .IsRequired(false);
        });

        modelBuilder.Entity<Droit>(entity =>
        {
            entity.ToTable("droits");

            entity.Property(d => d.Id);
            entity.Property(d => d.AssignedToUserId);
            entity.Property(d => d.Status);
            entity.Property(d => d.TriageNumber);
            entity.Property(d => d.ReportedDate);
            entity.Property(d => d.DateFound);
            entity.Property(d => d.OriginalSubmission);
            
            entity.Property(d => d.Reference).IsRequired();
            entity.HasIndex(d => d.Reference).IsUnique();

            entity.Property(d => d.IsHazardousFind);
            entity.Property(d => d.IsDredge);
            
            entity.Property(d => d.Created);
            entity.Property(d => d.LastModified);
            entity.Property(d => d.LastModifiedByUserId).IsRequired(false);
            

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
            entity.Property(d => d.MmoLicenceRequired);
            entity.Property(d => d.MmoLicenceProvided);
            entity.Property(d => d.SalvageClaimAwarded);

            // Legacy fields
            entity.Property(d => d.PowerappsDroitId);
            entity.Property(d => d.PowerappsWreckId);
            entity.Property(d => d.District);
            entity.Property(d => d.LegacyFileReference);
            entity.Property(d => d.GoodsDischargedBy);
            entity.Property(d => d.DateDelivered);
            entity.Property(d => d.Agent);
            entity.Property(d => d.RecoveredFrom);
            entity.Property(d => d.LegacyRemarks);
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
            
            entity.HasOne(d => d.LastModifiedByUser)
                .WithMany()
                .HasForeignKey(d => d.LastModifiedByUserId)
                .IsRequired(false);
            
            entity.HasOne(d => d.AssignedToUser)
                .WithMany()
                .HasForeignKey(d => d.AssignedToUserId)
                .IsRequired(false);
            
            entity.HasMany(d => d.Notes)
                .WithOne(n => n.Droit)
                .HasForeignKey(n => n.DroitId)
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
            entity.Property(w => w.SalvorValuation);
            entity.Property(w => w.ReceiverValuation);
            entity.Property(w => w.ValueConfirmed);
            entity.Property(w => w.WreckMaterialOwner);
            entity.Property(w => w.WreckMaterialOwnerContactDetails);
            entity.Property(w => w.Purchaser);
            entity.Property(w => w.PurchaserContactDetails);
            entity.Property(w => w.Outcome);
            entity.Property(w => w.OutcomeRemarks);
            
            entity.Property(w => w.Created);
            entity.Property(w => w.LastModified);
            entity.Property(w => w.LastModifiedByUserId).IsRequired(false);
            entity.Property(w => w.PowerappsWreckMaterialId);
            entity.Property(w => w.PowerappsDroitId);


            entity.OwnsOne(w => w.StorageAddress);
            entity.Property(w => w.StoredAtSalvorAddress);

            entity.HasOne(w => w.Droit)
                .WithMany(d => d.WreckMaterials)
                .HasForeignKey(w => w.DroitId)
                .IsRequired();

            entity.HasMany(w => w.Images)
                .WithOne(i => i.WreckMaterial)
                .HasForeignKey(i => i.WreckMaterialId)
                .IsRequired(false);
            
            entity.HasMany(w => w.Files)
                .WithOne(i => i.WreckMaterial)
                .HasForeignKey(i => i.WreckMaterialId)
                .IsRequired(false);


            entity.HasOne(w => w.LastModifiedByUser)
                .WithMany()
                .HasForeignKey(w => w.LastModifiedByUserId)
                .IsRequired(false);
            
            
        });

        modelBuilder.Entity<Image>(entity =>
        {
            entity.ToTable("images");

            entity.Property(i => i.Id);
            entity.Property(i => i.Title);
            
            entity.Property(i => i.Key);
            entity.Property(i => i.Filename);
            entity.Property(i => i.FileContentType);
            
            entity.Property(i => i.WreckMaterialId);
            
            entity.Property(i => i.Created);
            entity.Property(i => i.LastModified);
            entity.Property(i => i.LastModifiedByUserId).IsRequired(false);
            
            entity.HasOne(i => i.WreckMaterial)
                .WithMany(w => w.Images)
                .HasForeignKey(i => i.WreckMaterialId)
                .IsRequired(false);
            
            
        });

        
        modelBuilder.Entity<DroitFile>(entity =>
        {
            entity.ToTable("droit_files");

            entity.Property(i => i.Id);
            entity.Property(i => i.Title);
            entity.Property(i => i.Url);

            
            entity.Property(i => i.Key);
            entity.Property(i => i.Filename);
            entity.Property(i => i.FileContentType);
            
            entity.Property(i => i.WreckMaterialId);
            
            entity.Property(i => i.Created);
            entity.Property(i => i.LastModified);
            entity.Property(i => i.LastModifiedByUserId).IsRequired(false);
            
            entity.HasOne(i => i.WreckMaterial)
                .WithMany(w => w.Files)
                .HasForeignKey(i => i.WreckMaterialId)
                .IsRequired(false);
            
            entity.HasOne(i => i.Note)
                .WithMany(n => n.Files)
                .HasForeignKey(i => i.NoteId)
                .IsRequired(false);
            
           
        });


        modelBuilder.Entity<Wreck>(entity =>
        {
            entity.ToTable("wrecks");

            entity.Property(w => w.Id);
            entity.Property(w => w.Name);
            entity.Property(w => w.YearConstructed);
            entity.Property(w => w.ConstructionDetails);
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
            entity.Property(w => w.OwnerAddress);
            entity.Property(w => w.AdditionalInformation);
            
            entity.Property(w => w.Created);
            entity.Property(w => w.LastModified);
            entity.Property(w => w.LastModifiedByUserId).IsRequired(false);;
            entity.Property(w => w.PowerappsWreckId);
            entity.Property(w => w.WreckType);
            
            entity.HasMany(w => w.Droits)
                .WithOne(d => d.Wreck)
                .HasForeignKey(d => d.WreckId);
            
            entity.HasOne(w => w.LastModifiedByUser)
                .WithMany()
                .HasForeignKey(w => w.LastModifiedByUserId)
                .IsRequired(false);
            
            entity.HasMany(w => w.Notes)
                .WithOne(n => n.Wreck)
                .HasForeignKey(n => n.WreckId)
                .IsRequired(false);
        });

        modelBuilder.Entity<Letter>(entity =>
        {
            entity.ToTable("letters");

            entity.Property(l => l.Id);
            entity.Property(l => l.Subject);
            entity.Property(l => l.Body);
            entity.Property(l => l.DateSent);
            entity.Property(l => l.Recipient);
            entity.Property(l => l.Type);
            entity.Property(l => l.Status);
            
            entity.Property(l => l.Created);
            entity.Property(l => l.LastModified);
            entity.Property(l => l.LastModifiedByUserId).IsRequired(false);
            entity.Property(l => l.QualityApprovedUserId);

            
            entity.HasOne(l => l.Droit)
                .WithMany(d => d.Letters)
                .HasForeignKey(l => l.DroitId)
                .IsRequired(false);
            
            entity.HasOne(l => l.LastModifiedByUser)
                .WithMany()
                .HasForeignKey(l => l.LastModifiedByUserId)
                .IsRequired(false);
            
            
            entity.HasOne(l => l.QualityApprovedUser)
                .WithMany()
                .HasForeignKey(l => l.QualityApprovedUserId)
                .IsRequired(false);
            
            entity.HasMany(l => l.Notes)
                .WithOne(n => n.Letter)
                .HasForeignKey(n => n.LetterId)
                .IsRequired(false);
        });

        modelBuilder.Entity<Salvor>(entity =>
        {
            entity.ToTable("salvors");

            entity.Property(s => s.Id);
            entity.Property(s => s.PowerappsContactId);

            entity.Property(s => s.Email).IsRequired();
            
            entity.Property(s => s.Name);
            entity.Property(s => s.TelephoneNumber);
            entity.Property(s => s.MobileNumber);


            entity.Property(s => s.Created);
            entity.Property(s => s.LastModified);
            entity.Property(s => s.LastModifiedByUserId).IsRequired(false);

            entity.HasIndex(s => s.Email)
                .IsUnique();
            
            entity.OwnsOne(s => s.Address);
            entity.HasMany(s => s.Droits)
                .WithOne(d => d.Salvor)
                .HasForeignKey(d => d.SalvorId);
            
            entity.HasOne(s => s.LastModifiedByUser)
                .WithMany()
                .HasForeignKey(s => s.LastModifiedByUserId)
                .IsRequired(false);

            entity.HasMany(s => s.Notes)
                .WithOne(n => n.Salvor)
                .HasForeignKey(n => n.SalvorId)
                .IsRequired(false);

        });

        modelBuilder.HasDbFunction(
            typeof(CustomEfFunctions).GetMethod(
                nameof(CustomEfFunctions.SmallestLevenshteinDistance))!, builder =>
        {
            builder.HasParameter("source").HasStoreType("text");
            builder.HasParameter("target").HasStoreType("text");

            builder.HasTranslation(args =>
            {
                return new SqlFunctionExpression("get_smallest_levenshtein_distance", args, false, args.Select(x => false), typeof(int), null);
                
            });
        });
        
        base.OnModelCreating(modelBuilder);
    }


    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
