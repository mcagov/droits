START TRANSACTION;

ALTER TABLE droits ADD "ClosedDate" timestamp without time zone;

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20240415092217_AddedDroitClosedDate', '8.0.3');

COMMIT;

