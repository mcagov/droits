﻿// <auto-generated />
using System;
using Droits.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Droits.Data.Migrations
{
    [DbContext(typeof(DroitsContext))]
    [Migration("20240319154554_AddLevenshteinFunction")]
    partial class AddLevenshteinFunction
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.HasPostgresExtension(modelBuilder, "fuzzystrmatch");
            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Droits.Models.Entities.ApplicationUser", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("AuthId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("Created")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("LastModified")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("users", (string)null);
                });

            modelBuilder.Entity("Droits.Models.Entities.Droit", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Agent")
                        .HasColumnType("text");

                    b.Property<Guid?>("AssignedToUserId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("Created")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("DateDelivered")
                        .HasColumnType("text");

                    b.Property<DateTime>("DateFound")
                        .HasColumnType("timestamp without time zone");

                    b.Property<int?>("Depth")
                        .HasColumnType("integer");

                    b.Property<string>("District")
                        .HasColumnType("text");

                    b.Property<string>("GoodsDischargedBy")
                        .HasColumnType("text");

                    b.Property<bool>("ImportedFromLegacy")
                        .HasColumnType("boolean");

                    b.Property<bool>("InUkWaters")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsDredge")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsHazardousFind")
                        .HasColumnType("boolean");

                    b.Property<DateTime>("LastModified")
                        .HasColumnType("timestamp without time zone");

                    b.Property<Guid?>("LastModifiedByUserId")
                        .HasColumnType("uuid");

                    b.Property<double?>("Latitude")
                        .HasColumnType("double precision");

                    b.Property<string>("LegacyFileReference")
                        .HasColumnType("text");

                    b.Property<string>("LegacyRemarks")
                        .HasColumnType("text");

                    b.Property<string>("LocationDescription")
                        .HasColumnType("text");

                    b.Property<int?>("LocationRadius")
                        .HasColumnType("integer");

                    b.Property<double?>("Longitude")
                        .HasColumnType("double precision");

                    b.Property<bool>("MmoLicenceProvided")
                        .HasColumnType("boolean");

                    b.Property<bool>("MmoLicenceRequired")
                        .HasColumnType("boolean");

                    b.Property<string>("OriginalSubmission")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("PowerappsDroitId")
                        .HasColumnType("text");

                    b.Property<string>("PowerappsWreckId")
                        .HasColumnType("text");

                    b.Property<int?>("RecoveredFrom")
                        .HasColumnType("integer");

                    b.Property<string>("RecoveredFromLegacy")
                        .HasColumnType("text");

                    b.Property<string>("Reference")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("ReportedDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("ReportedWreckConstructionDetails")
                        .HasColumnType("text");

                    b.Property<string>("ReportedWreckName")
                        .HasColumnType("text");

                    b.Property<int?>("ReportedWreckYearConstructed")
                        .HasColumnType("integer");

                    b.Property<int?>("ReportedWreckYearSunk")
                        .HasColumnType("integer");

                    b.Property<bool>("SalvageAwardClaimed")
                        .HasColumnType("boolean");

                    b.Property<double>("SalvageClaimAwarded")
                        .HasColumnType("double precision");

                    b.Property<Guid?>("SalvorId")
                        .HasColumnType("uuid");

                    b.Property<string>("ServicesDescription")
                        .HasColumnType("text");

                    b.Property<string>("ServicesDuration")
                        .HasColumnType("text");

                    b.Property<double?>("ServicesEstimatedCost")
                        .HasColumnType("double precision");

                    b.Property<int>("Status")
                        .HasColumnType("integer");

                    b.Property<int?>("TriageNumber")
                        .HasColumnType("integer");

                    b.Property<Guid?>("WreckId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("AssignedToUserId");

                    b.HasIndex("LastModifiedByUserId");

                    b.HasIndex("Reference")
                        .IsUnique();

                    b.HasIndex("SalvorId");

                    b.HasIndex("WreckId");

                    b.ToTable("droits", (string)null);
                });

            modelBuilder.Entity("Droits.Models.Entities.DroitFile", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("Created")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("FileContentType")
                        .HasColumnType("text");

                    b.Property<string>("Filename")
                        .HasColumnType("text");

                    b.Property<string>("Key")
                        .HasColumnType("text");

                    b.Property<DateTime>("LastModified")
                        .HasColumnType("timestamp without time zone");

                    b.Property<Guid?>("LastModifiedByUserId")
                        .HasColumnType("uuid");

                    b.Property<Guid?>("NoteId")
                        .HasColumnType("uuid");

                    b.Property<string>("Title")
                        .HasColumnType("text");

                    b.Property<string>("Url")
                        .HasColumnType("text");

                    b.Property<Guid?>("WreckMaterialId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("LastModifiedByUserId");

                    b.HasIndex("NoteId");

                    b.HasIndex("WreckMaterialId");

                    b.ToTable("droit_files", (string)null);
                });

            modelBuilder.Entity("Droits.Models.Entities.Image", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("Created")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("FileContentType")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Filename")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Key")
                        .HasColumnType("text");

                    b.Property<DateTime>("LastModified")
                        .HasColumnType("timestamp without time zone");

                    b.Property<Guid?>("LastModifiedByUserId")
                        .HasColumnType("uuid");

                    b.Property<string>("Title")
                        .HasColumnType("text");

                    b.Property<Guid?>("WreckMaterialId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("LastModifiedByUserId");

                    b.HasIndex("WreckMaterialId");

                    b.ToTable("images", (string)null);
                });

            modelBuilder.Entity("Droits.Models.Entities.Letter", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Body")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("Created")
                        .HasColumnType("timestamp without time zone");

                    b.Property<DateTime?>("DateSent")
                        .HasColumnType("timestamp without time zone");

                    b.Property<Guid>("DroitId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("LastModified")
                        .HasColumnType("timestamp without time zone");

                    b.Property<Guid?>("LastModifiedByUserId")
                        .HasColumnType("uuid");

                    b.Property<Guid?>("QualityApprovedUserId")
                        .HasColumnType("uuid");

                    b.Property<string>("Recipient")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("SenderUserId")
                        .HasColumnType("uuid");

                    b.Property<int>("Status")
                        .HasColumnType("integer");

                    b.Property<string>("Subject")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("Type")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("DroitId");

                    b.HasIndex("LastModifiedByUserId");

                    b.HasIndex("QualityApprovedUserId");

                    b.ToTable("letters", (string)null);
                });

            modelBuilder.Entity("Droits.Models.Entities.Note", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("Created")
                        .HasColumnType("timestamp without time zone");

                    b.Property<Guid?>("DroitId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("LastModified")
                        .HasColumnType("timestamp without time zone");

                    b.Property<Guid?>("LastModifiedByUserId")
                        .HasColumnType("uuid");

                    b.Property<Guid?>("LetterId")
                        .HasColumnType("uuid");

                    b.Property<Guid?>("SalvorId")
                        .HasColumnType("uuid");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("Type")
                        .HasColumnType("integer");

                    b.Property<Guid?>("WreckId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("DroitId");

                    b.HasIndex("LastModifiedByUserId");

                    b.HasIndex("LetterId");

                    b.HasIndex("SalvorId");

                    b.HasIndex("WreckId");

                    b.ToTable("notes", (string)null);
                });

            modelBuilder.Entity("Droits.Models.Entities.Salvor", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("Created")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("LastModified")
                        .HasColumnType("timestamp without time zone");

                    b.Property<Guid?>("LastModifiedByUserId")
                        .HasColumnType("uuid");

                    b.Property<string>("MobileNumber")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("PowerappsContactId")
                        .HasColumnType("text");

                    b.Property<string>("TelephoneNumber")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.HasIndex("LastModifiedByUserId");

                    b.ToTable("salvors", (string)null);
                });

            modelBuilder.Entity("Droits.Models.Entities.Wreck", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("AdditionalInformation")
                        .HasColumnType("text");

                    b.Property<string>("ConstructionDetails")
                        .HasColumnType("text");

                    b.Property<DateTime>("Created")
                        .HasColumnType("timestamp without time zone");

                    b.Property<DateTime?>("DateOfLoss")
                        .HasColumnType("timestamp without time zone");

                    b.Property<bool>("InUkWaters")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsAnAircraft")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsProtectedSite")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsWarWreck")
                        .HasColumnType("boolean");

                    b.Property<DateTime>("LastModified")
                        .HasColumnType("timestamp without time zone");

                    b.Property<Guid?>("LastModifiedByUserId")
                        .HasColumnType("uuid");

                    b.Property<double?>("Latitude")
                        .HasColumnType("double precision");

                    b.Property<double?>("Longitude")
                        .HasColumnType("double precision");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("OwnerAddress")
                        .HasColumnType("text");

                    b.Property<string>("OwnerEmail")
                        .HasColumnType("text");

                    b.Property<string>("OwnerName")
                        .HasColumnType("text");

                    b.Property<string>("OwnerNumber")
                        .HasColumnType("text");

                    b.Property<string>("PowerappsWreckId")
                        .HasColumnType("text");

                    b.Property<string>("ProtectionLegislation")
                        .HasColumnType("text");

                    b.Property<int?>("WreckType")
                        .HasColumnType("integer");

                    b.Property<int?>("YearConstructed")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("LastModifiedByUserId");

                    b.ToTable("wrecks", (string)null);
                });

            modelBuilder.Entity("Droits.Models.Entities.WreckMaterial", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("Created")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<Guid>("DroitId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("LastModified")
                        .HasColumnType("timestamp without time zone");

                    b.Property<Guid?>("LastModifiedByUserId")
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int?>("Outcome")
                        .HasColumnType("integer");

                    b.Property<string>("OutcomeRemarks")
                        .HasColumnType("text");

                    b.Property<string>("PowerappsDroitId")
                        .HasColumnType("text");

                    b.Property<string>("PowerappsWreckMaterialId")
                        .HasColumnType("text");

                    b.Property<string>("Purchaser")
                        .HasColumnType("text");

                    b.Property<string>("PurchaserContactDetails")
                        .HasColumnType("text");

                    b.Property<int>("Quantity")
                        .HasColumnType("integer");

                    b.Property<double?>("ReceiverValuation")
                        .HasColumnType("double precision");

                    b.Property<double?>("SalvorValuation")
                        .HasColumnType("double precision");

                    b.Property<bool>("StoredAtSalvorAddress")
                        .HasColumnType("boolean");

                    b.Property<bool>("ValueConfirmed")
                        .HasColumnType("boolean");

                    b.Property<bool>("ValueKnown")
                        .HasColumnType("boolean");

                    b.Property<string>("WreckMaterialOwner")
                        .HasColumnType("text");

                    b.Property<string>("WreckMaterialOwnerContactDetails")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("DroitId");

                    b.HasIndex("LastModifiedByUserId");

                    b.ToTable("wreck_materials", (string)null);
                });

            modelBuilder.Entity("Droits.Models.Entities.Droit", b =>
                {
                    b.HasOne("Droits.Models.Entities.ApplicationUser", "AssignedToUser")
                        .WithMany()
                        .HasForeignKey("AssignedToUserId");

                    b.HasOne("Droits.Models.Entities.ApplicationUser", "LastModifiedByUser")
                        .WithMany()
                        .HasForeignKey("LastModifiedByUserId");

                    b.HasOne("Droits.Models.Entities.Salvor", "Salvor")
                        .WithMany("Droits")
                        .HasForeignKey("SalvorId");

                    b.HasOne("Droits.Models.Entities.Wreck", "Wreck")
                        .WithMany("Droits")
                        .HasForeignKey("WreckId");

                    b.Navigation("AssignedToUser");

                    b.Navigation("LastModifiedByUser");

                    b.Navigation("Salvor");

                    b.Navigation("Wreck");
                });

            modelBuilder.Entity("Droits.Models.Entities.DroitFile", b =>
                {
                    b.HasOne("Droits.Models.Entities.ApplicationUser", "LastModifiedByUser")
                        .WithMany()
                        .HasForeignKey("LastModifiedByUserId");

                    b.HasOne("Droits.Models.Entities.Note", "Note")
                        .WithMany("Files")
                        .HasForeignKey("NoteId");

                    b.HasOne("Droits.Models.Entities.WreckMaterial", "WreckMaterial")
                        .WithMany("Files")
                        .HasForeignKey("WreckMaterialId");

                    b.Navigation("LastModifiedByUser");

                    b.Navigation("Note");

                    b.Navigation("WreckMaterial");
                });

            modelBuilder.Entity("Droits.Models.Entities.Image", b =>
                {
                    b.HasOne("Droits.Models.Entities.ApplicationUser", "LastModifiedByUser")
                        .WithMany()
                        .HasForeignKey("LastModifiedByUserId");

                    b.HasOne("Droits.Models.Entities.WreckMaterial", "WreckMaterial")
                        .WithMany("Images")
                        .HasForeignKey("WreckMaterialId");

                    b.Navigation("LastModifiedByUser");

                    b.Navigation("WreckMaterial");
                });

            modelBuilder.Entity("Droits.Models.Entities.Letter", b =>
                {
                    b.HasOne("Droits.Models.Entities.Droit", "Droit")
                        .WithMany("Letters")
                        .HasForeignKey("DroitId");

                    b.HasOne("Droits.Models.Entities.ApplicationUser", "LastModifiedByUser")
                        .WithMany()
                        .HasForeignKey("LastModifiedByUserId");

                    b.HasOne("Droits.Models.Entities.ApplicationUser", "QualityApprovedUser")
                        .WithMany()
                        .HasForeignKey("QualityApprovedUserId");

                    b.Navigation("Droit");

                    b.Navigation("LastModifiedByUser");

                    b.Navigation("QualityApprovedUser");
                });

            modelBuilder.Entity("Droits.Models.Entities.Note", b =>
                {
                    b.HasOne("Droits.Models.Entities.Droit", "Droit")
                        .WithMany("Notes")
                        .HasForeignKey("DroitId");

                    b.HasOne("Droits.Models.Entities.ApplicationUser", "LastModifiedByUser")
                        .WithMany()
                        .HasForeignKey("LastModifiedByUserId");

                    b.HasOne("Droits.Models.Entities.Letter", "Letter")
                        .WithMany("Notes")
                        .HasForeignKey("LetterId");

                    b.HasOne("Droits.Models.Entities.Salvor", "Salvor")
                        .WithMany("Notes")
                        .HasForeignKey("SalvorId");

                    b.HasOne("Droits.Models.Entities.Wreck", "Wreck")
                        .WithMany("Notes")
                        .HasForeignKey("WreckId");

                    b.Navigation("Droit");

                    b.Navigation("LastModifiedByUser");

                    b.Navigation("Letter");

                    b.Navigation("Salvor");

                    b.Navigation("Wreck");
                });

            modelBuilder.Entity("Droits.Models.Entities.Salvor", b =>
                {
                    b.HasOne("Droits.Models.Entities.ApplicationUser", "LastModifiedByUser")
                        .WithMany()
                        .HasForeignKey("LastModifiedByUserId");

                    b.OwnsOne("Droits.Models.Entities.Address", "Address", b1 =>
                        {
                            b1.Property<Guid>("SalvorId")
                                .HasColumnType("uuid");

                            b1.Property<string>("County")
                                .HasColumnType("text");

                            b1.Property<string>("Line1")
                                .HasColumnType("text");

                            b1.Property<string>("Line2")
                                .HasColumnType("text");

                            b1.Property<string>("Postcode")
                                .HasColumnType("text");

                            b1.Property<string>("Town")
                                .HasColumnType("text");

                            b1.HasKey("SalvorId");

                            b1.ToTable("salvors");

                            b1.WithOwner()
                                .HasForeignKey("SalvorId");
                        });

                    b.Navigation("Address")
                        .IsRequired();

                    b.Navigation("LastModifiedByUser");
                });

            modelBuilder.Entity("Droits.Models.Entities.Wreck", b =>
                {
                    b.HasOne("Droits.Models.Entities.ApplicationUser", "LastModifiedByUser")
                        .WithMany()
                        .HasForeignKey("LastModifiedByUserId");

                    b.Navigation("LastModifiedByUser");
                });

            modelBuilder.Entity("Droits.Models.Entities.WreckMaterial", b =>
                {
                    b.HasOne("Droits.Models.Entities.Droit", "Droit")
                        .WithMany("WreckMaterials")
                        .HasForeignKey("DroitId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Droits.Models.Entities.ApplicationUser", "LastModifiedByUser")
                        .WithMany()
                        .HasForeignKey("LastModifiedByUserId");

                    b.OwnsOne("Droits.Models.Entities.Address", "StorageAddress", b1 =>
                        {
                            b1.Property<Guid>("WreckMaterialId")
                                .HasColumnType("uuid");

                            b1.Property<string>("County")
                                .HasColumnType("text");

                            b1.Property<string>("Line1")
                                .HasColumnType("text");

                            b1.Property<string>("Line2")
                                .HasColumnType("text");

                            b1.Property<string>("Postcode")
                                .HasColumnType("text");

                            b1.Property<string>("Town")
                                .HasColumnType("text");

                            b1.HasKey("WreckMaterialId");

                            b1.ToTable("wreck_materials");

                            b1.WithOwner()
                                .HasForeignKey("WreckMaterialId");
                        });

                    b.Navigation("Droit");

                    b.Navigation("LastModifiedByUser");

                    b.Navigation("StorageAddress")
                        .IsRequired();
                });

            modelBuilder.Entity("Droits.Models.Entities.Droit", b =>
                {
                    b.Navigation("Letters");

                    b.Navigation("Notes");

                    b.Navigation("WreckMaterials");
                });

            modelBuilder.Entity("Droits.Models.Entities.Letter", b =>
                {
                    b.Navigation("Notes");
                });

            modelBuilder.Entity("Droits.Models.Entities.Note", b =>
                {
                    b.Navigation("Files");
                });

            modelBuilder.Entity("Droits.Models.Entities.Salvor", b =>
                {
                    b.Navigation("Droits");

                    b.Navigation("Notes");
                });

            modelBuilder.Entity("Droits.Models.Entities.Wreck", b =>
                {
                    b.Navigation("Droits");

                    b.Navigation("Notes");
                });

            modelBuilder.Entity("Droits.Models.Entities.WreckMaterial", b =>
                {
                    b.Navigation("Files");

                    b.Navigation("Images");
                });
#pragma warning restore 612, 618
        }
    }
}
