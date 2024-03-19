using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Droits.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:PostgresExtension:fuzzystrmatch", ",,");


            if ( !migrationBuilder.TableExists("users") )
            {
                migrationBuilder.CreateTable(
                    name: "users",
                    columns: table => new
                    {
                        Id = table.Column<Guid>(type: "uuid", nullable: false),
                        Name = table.Column<string>(type: "text", nullable: false),
                        AuthId = table.Column<string>(type: "text", nullable: false),
                        Email = table.Column<string>(type: "text", nullable: false),
                        Created = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                        LastModified = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                    },
                    constraints: table =>
                    {
                        table.PrimaryKey("PK_users", x => x.Id);
                    });
            }

            if ( !migrationBuilder.TableExists("salvors") )
            {
                migrationBuilder.CreateTable(
                    name: "salvors",
                    columns: table => new
                    {
                        Id = table.Column<Guid>(type: "uuid", nullable: false),
                        Email = table.Column<string>(type: "text", nullable: false),
                        Name = table.Column<string>(type: "text", nullable: false),
                        PowerappsContactId = table.Column<string>(type: "text", nullable: true),
                        TelephoneNumber = table.Column<string>(type: "text", nullable: true),
                        MobileNumber = table.Column<string>(type: "text", nullable: true),
                        Address_Line1 = table.Column<string>(type: "text", nullable: true),
                        Address_Line2 = table.Column<string>(type: "text", nullable: true),
                        Address_Town = table.Column<string>(type: "text", nullable: true),
                        Address_County = table.Column<string>(type: "text", nullable: true),
                        Address_Postcode = table.Column<string>(type: "text", nullable: true),
                        Created = table.Column<DateTime>(type: "timestamp without time zone",
                            nullable: false),
                        LastModified = table.Column<DateTime>(type: "timestamp without time zone",
                            nullable: false),
                        LastModifiedByUserId = table.Column<Guid>(type: "uuid", nullable: true)
                    },
                    constraints: table =>
                    {
                        table.PrimaryKey("PK_salvors", x => x.Id);
                        table.ForeignKey(
                            name: "FK_salvors_users_LastModifiedByUserId",
                            column: x => x.LastModifiedByUserId,
                            principalTable: "users",
                            principalColumn: "Id");
                    });
            }

            if ( !migrationBuilder.TableExists("wrecks") )
            {
                migrationBuilder.CreateTable(
                    name: "wrecks",
                    columns: table => new
                    {
                        Id = table.Column<Guid>(type: "uuid", nullable: false),
                        PowerappsWreckId = table.Column<string>(type: "text", nullable: true),
                        Name = table.Column<string>(type: "text", nullable: false),
                        WreckType = table.Column<int>(type: "integer", nullable: true),
                        ConstructionDetails = table.Column<string>(type: "text", nullable: true),
                        YearConstructed = table.Column<int>(type: "integer", nullable: true),
                        DateOfLoss = table.Column<DateTime>(type: "timestamp without time zone",
                            nullable: true),
                        InUkWaters = table.Column<bool>(type: "boolean", nullable: false),
                        IsWarWreck = table.Column<bool>(type: "boolean", nullable: false),
                        IsAnAircraft = table.Column<bool>(type: "boolean", nullable: false),
                        Latitude = table.Column<double>(type: "double precision", nullable: true),
                        Longitude = table.Column<double>(type: "double precision", nullable: true),
                        IsProtectedSite = table.Column<bool>(type: "boolean", nullable: false),
                        ProtectionLegislation = table.Column<string>(type: "text", nullable: true),
                        OwnerName = table.Column<string>(type: "text", nullable: true),
                        OwnerEmail = table.Column<string>(type: "text", nullable: true),
                        OwnerNumber = table.Column<string>(type: "text", nullable: true),
                        OwnerAddress = table.Column<string>(type: "text", nullable: true),
                        AdditionalInformation = table.Column<string>(type: "text", nullable: true),
                        Created = table.Column<DateTime>(type: "timestamp without time zone",
                            nullable: false),
                        LastModified = table.Column<DateTime>(type: "timestamp without time zone",
                            nullable: false),
                        LastModifiedByUserId = table.Column<Guid>(type: "uuid", nullable: true)
                    },
                    constraints: table =>
                    {
                        table.PrimaryKey("PK_wrecks", x => x.Id);
                        table.ForeignKey(
                            name: "FK_wrecks_users_LastModifiedByUserId",
                            column: x => x.LastModifiedByUserId,
                            principalTable: "users",
                            principalColumn: "Id");
                    });
            }

            if ( !migrationBuilder.TableExists("droits") )
            {
                migrationBuilder.CreateTable(
                    name: "droits",
                    columns: table => new
                    {
                        Id = table.Column<Guid>(type: "uuid", nullable: false),
                        AssignedToUserId = table.Column<Guid>(type: "uuid", nullable: true),
                        Reference = table.Column<string>(type: "text", nullable: false),
                        Status = table.Column<int>(type: "integer", nullable: false),
                        TriageNumber = table.Column<int>(type: "integer", nullable: true),
                        ReportedDate = table.Column<DateTime>(type: "timestamp without time zone",
                            nullable: false),
                        DateFound = table.Column<DateTime>(type: "timestamp without time zone",
                            nullable: false),
                        OriginalSubmission = table.Column<string>(type: "text", nullable: false),
                        WreckId = table.Column<Guid>(type: "uuid", nullable: true),
                        IsHazardousFind = table.Column<bool>(type: "boolean", nullable: false),
                        IsDredge = table.Column<bool>(type: "boolean", nullable: false),
                        ReportedWreckName = table.Column<string>(type: "text", nullable: true),
                        ReportedWreckYearSunk = table.Column<int>(type: "integer", nullable: true),
                        ReportedWreckYearConstructed =
                            table.Column<int>(type: "integer", nullable: true),
                        ReportedWreckConstructionDetails =
                            table.Column<string>(type: "text", nullable: true),
                        SalvorId = table.Column<Guid>(type: "uuid", nullable: true),
                        Latitude = table.Column<double>(type: "double precision", nullable: true),
                        Longitude = table.Column<double>(type: "double precision", nullable: true),
                        InUkWaters = table.Column<bool>(type: "boolean", nullable: false),
                        LocationRadius = table.Column<int>(type: "integer", nullable: true),
                        Depth = table.Column<int>(type: "integer", nullable: true),
                        RecoveredFrom = table.Column<int>(type: "integer", nullable: true),
                        LocationDescription = table.Column<string>(type: "text", nullable: true),
                        SalvageAwardClaimed = table.Column<bool>(type: "boolean", nullable: false),
                        ServicesDescription = table.Column<string>(type: "text", nullable: true),
                        ServicesDuration = table.Column<string>(type: "text", nullable: true),
                        ServicesEstimatedCost =
                            table.Column<double>(type: "double precision", nullable: true),
                        MmoLicenceRequired = table.Column<bool>(type: "boolean", nullable: false),
                        MmoLicenceProvided = table.Column<bool>(type: "boolean", nullable: false),
                        SalvageClaimAwarded =
                            table.Column<double>(type: "double precision", nullable: false),
                        PowerappsDroitId = table.Column<string>(type: "text", nullable: true),
                        PowerappsWreckId = table.Column<string>(type: "text", nullable: true),
                        District = table.Column<string>(type: "text", nullable: true),
                        LegacyFileReference = table.Column<string>(type: "text", nullable: true),
                        GoodsDischargedBy = table.Column<string>(type: "text", nullable: true),
                        DateDelivered = table.Column<string>(type: "text", nullable: true),
                        Agent = table.Column<string>(type: "text", nullable: true),
                        RecoveredFromLegacy = table.Column<string>(type: "text", nullable: true),
                        LegacyRemarks = table.Column<string>(type: "text", nullable: true),
                        ImportedFromLegacy = table.Column<bool>(type: "boolean", nullable: false),
                        Created = table.Column<DateTime>(type: "timestamp without time zone",
                            nullable: false),
                        LastModified = table.Column<DateTime>(type: "timestamp without time zone",
                            nullable: false),
                        LastModifiedByUserId = table.Column<Guid>(type: "uuid", nullable: true)
                    },
                    constraints: table =>
                    {
                        table.PrimaryKey("PK_droits", x => x.Id);
                        table.ForeignKey(
                            name: "FK_droits_salvors_SalvorId",
                            column: x => x.SalvorId,
                            principalTable: "salvors",
                            principalColumn: "Id");
                        table.ForeignKey(
                            name: "FK_droits_users_AssignedToUserId",
                            column: x => x.AssignedToUserId,
                            principalTable: "users",
                            principalColumn: "Id");
                        table.ForeignKey(
                            name: "FK_droits_users_LastModifiedByUserId",
                            column: x => x.LastModifiedByUserId,
                            principalTable: "users",
                            principalColumn: "Id");
                        table.ForeignKey(
                            name: "FK_droits_wrecks_WreckId",
                            column: x => x.WreckId,
                            principalTable: "wrecks",
                            principalColumn: "Id");
                    });
            }

            if ( !migrationBuilder.TableExists("letters") )
            {
                migrationBuilder.CreateTable(
                    name: "letters",
                    columns: table => new
                    {
                        Id = table.Column<Guid>(type: "uuid", nullable: false),
                        DroitId = table.Column<Guid>(type: "uuid", nullable: false),
                        QualityApprovedUserId = table.Column<Guid>(type: "uuid", nullable: true),
                        Subject = table.Column<string>(type: "text", nullable: false),
                        Body = table.Column<string>(type: "text", nullable: false),
                        Recipient = table.Column<string>(type: "text", nullable: false),
                        Status = table.Column<int>(type: "integer", nullable: false),
                        Type = table.Column<int>(type: "integer", nullable: false),
                        SenderUserId = table.Column<Guid>(type: "uuid", nullable: false),
                        DateSent = table.Column<DateTime>(type: "timestamp without time zone",
                            nullable: true),
                        Created = table.Column<DateTime>(type: "timestamp without time zone",
                            nullable: false),
                        LastModified = table.Column<DateTime>(type: "timestamp without time zone",
                            nullable: false),
                        LastModifiedByUserId = table.Column<Guid>(type: "uuid", nullable: true)
                    },
                    constraints: table =>
                    {
                        table.PrimaryKey("PK_letters", x => x.Id);
                        table.ForeignKey(
                            name: "FK_letters_droits_DroitId",
                            column: x => x.DroitId,
                            principalTable: "droits",
                            principalColumn: "Id");
                        table.ForeignKey(
                            name: "FK_letters_users_LastModifiedByUserId",
                            column: x => x.LastModifiedByUserId,
                            principalTable: "users",
                            principalColumn: "Id");
                        table.ForeignKey(
                            name: "FK_letters_users_QualityApprovedUserId",
                            column: x => x.QualityApprovedUserId,
                            principalTable: "users",
                            principalColumn: "Id");
                    });
            }

            if ( !migrationBuilder.TableExists("wreck_materials") )
            {
                migrationBuilder.CreateTable(
                    name: "wreck_materials",
                    columns: table => new
                    {
                        Id = table.Column<Guid>(type: "uuid", nullable: false),
                        DroitId = table.Column<Guid>(type: "uuid", nullable: false),
                        Name = table.Column<string>(type: "text", nullable: false),
                        StorageAddress_Line1 = table.Column<string>(type: "text", nullable: true),
                        StorageAddress_Line2 = table.Column<string>(type: "text", nullable: true),
                        StorageAddress_Town = table.Column<string>(type: "text", nullable: true),
                        StorageAddress_County = table.Column<string>(type: "text", nullable: true),
                        StorageAddress_Postcode =
                            table.Column<string>(type: "text", nullable: true),
                        StoredAtSalvorAddress =
                            table.Column<bool>(type: "boolean", nullable: false),
                        Description = table.Column<string>(type: "text", nullable: true),
                        Quantity = table.Column<int>(type: "integer", nullable: false),
                        SalvorValuation =
                            table.Column<double>(type: "double precision", nullable: true),
                        ValueKnown = table.Column<bool>(type: "boolean", nullable: false),
                        ReceiverValuation =
                            table.Column<double>(type: "double precision", nullable: true),
                        ValueConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                        WreckMaterialOwner = table.Column<string>(type: "text", nullable: true),
                        WreckMaterialOwnerContactDetails =
                            table.Column<string>(type: "text", nullable: true),
                        Purchaser = table.Column<string>(type: "text", nullable: true),
                        PurchaserContactDetails =
                            table.Column<string>(type: "text", nullable: true),
                        Outcome = table.Column<int>(type: "integer", nullable: true),
                        OutcomeRemarks = table.Column<string>(type: "text", nullable: true),
                        PowerappsWreckMaterialId =
                            table.Column<string>(type: "text", nullable: true),
                        PowerappsDroitId = table.Column<string>(type: "text", nullable: true),
                        Created = table.Column<DateTime>(type: "timestamp without time zone",
                            nullable: false),
                        LastModified = table.Column<DateTime>(type: "timestamp without time zone",
                            nullable: false),
                        LastModifiedByUserId = table.Column<Guid>(type: "uuid", nullable: true)
                    },
                    constraints: table =>
                    {
                        table.PrimaryKey("PK_wreck_materials", x => x.Id);
                        table.ForeignKey(
                            name: "FK_wreck_materials_droits_DroitId",
                            column: x => x.DroitId,
                            principalTable: "droits",
                            principalColumn: "Id",
                            onDelete: ReferentialAction.Cascade);
                        table.ForeignKey(
                            name: "FK_wreck_materials_users_LastModifiedByUserId",
                            column: x => x.LastModifiedByUserId,
                            principalTable: "users",
                            principalColumn: "Id");
                    });
            }

            if ( !migrationBuilder.TableExists("notes") )
            {

                migrationBuilder.CreateTable(
                    name: "notes",
                    columns: table => new
                    {
                        Id = table.Column<Guid>(type: "uuid", nullable: false),
                        Type = table.Column<int>(type: "integer", nullable: false),
                        Title = table.Column<string>(type: "text", nullable: false),
                        Text = table.Column<string>(type: "text", nullable: false),
                        DroitId = table.Column<Guid>(type: "uuid", nullable: true),
                        WreckId = table.Column<Guid>(type: "uuid", nullable: true),
                        SalvorId = table.Column<Guid>(type: "uuid", nullable: true),
                        LetterId = table.Column<Guid>(type: "uuid", nullable: true),
                        Created = table.Column<DateTime>(type: "timestamp without time zone",
                            nullable: false),
                        LastModified = table.Column<DateTime>(type: "timestamp without time zone",
                            nullable: false),
                        LastModifiedByUserId = table.Column<Guid>(type: "uuid", nullable: true)
                    },
                    constraints: table =>
                    {
                        table.PrimaryKey("PK_notes", x => x.Id);
                        table.ForeignKey(
                            name: "FK_notes_droits_DroitId",
                            column: x => x.DroitId,
                            principalTable: "droits",
                            principalColumn: "Id");
                        table.ForeignKey(
                            name: "FK_notes_letters_LetterId",
                            column: x => x.LetterId,
                            principalTable: "letters",
                            principalColumn: "Id");
                        table.ForeignKey(
                            name: "FK_notes_salvors_SalvorId",
                            column: x => x.SalvorId,
                            principalTable: "salvors",
                            principalColumn: "Id");
                        table.ForeignKey(
                            name: "FK_notes_users_LastModifiedByUserId",
                            column: x => x.LastModifiedByUserId,
                            principalTable: "users",
                            principalColumn: "Id");
                        table.ForeignKey(
                            name: "FK_notes_wrecks_WreckId",
                            column: x => x.WreckId,
                            principalTable: "wrecks",
                            principalColumn: "Id");
                    });
            }

            if ( !migrationBuilder.TableExists("images") )
            {

                migrationBuilder.CreateTable(
                    name: "images",
                    columns: table => new
                    {
                        Id = table.Column<Guid>(type: "uuid", nullable: false),
                        Title = table.Column<string>(type: "text", nullable: true),
                        Key = table.Column<string>(type: "text", nullable: true),
                        Filename = table.Column<string>(type: "text", nullable: false),
                        FileContentType = table.Column<string>(type: "text", nullable: false),
                        WreckMaterialId = table.Column<Guid>(type: "uuid", nullable: true),
                        Created = table.Column<DateTime>(type: "timestamp without time zone",
                            nullable: false),
                        LastModified = table.Column<DateTime>(type: "timestamp without time zone",
                            nullable: false),
                        LastModifiedByUserId = table.Column<Guid>(type: "uuid", nullable: true)
                    },
                    constraints: table =>
                    {
                        table.PrimaryKey("PK_images", x => x.Id);
                        table.ForeignKey(
                            name: "FK_images_users_LastModifiedByUserId",
                            column: x => x.LastModifiedByUserId,
                            principalTable: "users",
                            principalColumn: "Id");
                        table.ForeignKey(
                            name: "FK_images_wreck_materials_WreckMaterialId",
                            column: x => x.WreckMaterialId,
                            principalTable: "wreck_materials",
                            principalColumn: "Id");
                    });
            }

            if ( !migrationBuilder.TableExists("droit_files") )
            {


                migrationBuilder.CreateTable(
                    name: "droit_files",
                    columns: table => new
                    {
                        Id = table.Column<Guid>(type: "uuid", nullable: false),
                        Title = table.Column<string>(type: "text", nullable: true),
                        Url = table.Column<string>(type: "text", nullable: true),
                        Key = table.Column<string>(type: "text", nullable: true),
                        Filename = table.Column<string>(type: "text", nullable: true),
                        FileContentType = table.Column<string>(type: "text", nullable: true),
                        WreckMaterialId = table.Column<Guid>(type: "uuid", nullable: true),
                        NoteId = table.Column<Guid>(type: "uuid", nullable: true),
                        Created = table.Column<DateTime>(type: "timestamp without time zone",
                            nullable: false),
                        LastModified = table.Column<DateTime>(type: "timestamp without time zone",
                            nullable: false),
                        LastModifiedByUserId = table.Column<Guid>(type: "uuid", nullable: true)
                    },
                    constraints: table =>
                    {
                        table.PrimaryKey("PK_droit_files", x => x.Id);
                        table.ForeignKey(
                            name: "FK_droit_files_notes_NoteId",
                            column: x => x.NoteId,
                            principalTable: "notes",
                            principalColumn: "Id");
                        table.ForeignKey(
                            name: "FK_droit_files_users_LastModifiedByUserId",
                            column: x => x.LastModifiedByUserId,
                            principalTable: "users",
                            principalColumn: "Id");
                        table.ForeignKey(
                            name: "FK_droit_files_wreck_materials_WreckMaterialId",
                            column: x => x.WreckMaterialId,
                            principalTable: "wreck_materials",
                            principalColumn: "Id");
                    });
            }

            migrationBuilder.CreateIndex(
                name: "IX_droit_files_LastModifiedByUserId",
                table: "droit_files",
                column: "LastModifiedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_droit_files_NoteId",
                table: "droit_files",
                column: "NoteId");

            migrationBuilder.CreateIndex(
                name: "IX_droit_files_WreckMaterialId",
                table: "droit_files",
                column: "WreckMaterialId");

            migrationBuilder.CreateIndex(
                name: "IX_droits_AssignedToUserId",
                table: "droits",
                column: "AssignedToUserId");

            migrationBuilder.CreateIndex(
                name: "IX_droits_LastModifiedByUserId",
                table: "droits",
                column: "LastModifiedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_droits_Reference",
                table: "droits",
                column: "Reference",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_droits_SalvorId",
                table: "droits",
                column: "SalvorId");

            migrationBuilder.CreateIndex(
                name: "IX_droits_WreckId",
                table: "droits",
                column: "WreckId");

            migrationBuilder.CreateIndex(
                name: "IX_images_LastModifiedByUserId",
                table: "images",
                column: "LastModifiedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_images_WreckMaterialId",
                table: "images",
                column: "WreckMaterialId");

            migrationBuilder.CreateIndex(
                name: "IX_letters_DroitId",
                table: "letters",
                column: "DroitId");

            migrationBuilder.CreateIndex(
                name: "IX_letters_LastModifiedByUserId",
                table: "letters",
                column: "LastModifiedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_letters_QualityApprovedUserId",
                table: "letters",
                column: "QualityApprovedUserId");

            migrationBuilder.CreateIndex(
                name: "IX_notes_DroitId",
                table: "notes",
                column: "DroitId");

            migrationBuilder.CreateIndex(
                name: "IX_notes_LastModifiedByUserId",
                table: "notes",
                column: "LastModifiedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_notes_LetterId",
                table: "notes",
                column: "LetterId");

            migrationBuilder.CreateIndex(
                name: "IX_notes_SalvorId",
                table: "notes",
                column: "SalvorId");

            migrationBuilder.CreateIndex(
                name: "IX_notes_WreckId",
                table: "notes",
                column: "WreckId");

            migrationBuilder.CreateIndex(
                name: "IX_salvors_Email",
                table: "salvors",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_salvors_LastModifiedByUserId",
                table: "salvors",
                column: "LastModifiedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_wreck_materials_DroitId",
                table: "wreck_materials",
                column: "DroitId");

            migrationBuilder.CreateIndex(
                name: "IX_wreck_materials_LastModifiedByUserId",
                table: "wreck_materials",
                column: "LastModifiedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_wrecks_LastModifiedByUserId",
                table: "wrecks",
                column: "LastModifiedByUserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "droit_files");

            migrationBuilder.DropTable(
                name: "images");

            migrationBuilder.DropTable(
                name: "notes");

            migrationBuilder.DropTable(
                name: "wreck_materials");

            migrationBuilder.DropTable(
                name: "letters");

            migrationBuilder.DropTable(
                name: "droits");

            migrationBuilder.DropTable(
                name: "salvors");

            migrationBuilder.DropTable(
                name: "wrecks");

            migrationBuilder.DropTable(
                name: "users");
        }
    }
}
