BEGIN;

CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory" (
    "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL,
    CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId")
);

CREATE EXTENSION IF NOT EXISTS fuzzystrmatch;

CREATE OR REPLACE FUNCTION get_smallest_levenshtein_distance(source_text text, target_text text)
RETURNS INT AS $$
DECLARE
    source_whole_distance INT;
    min_distance INT := NULL;
    target_parts text[];
    word_distance INT;
    target_part text;
BEGIN
    source_whole_distance := LEVENSHTEIN(source_text, target_text);
    target_parts := string_to_array(target_text, ' ');
    min_distance := NULL;

    FOR i IN 1..array_length(target_parts, 1) LOOP
        target_part := target_parts[i];
        word_distance := LEVENSHTEIN(source_text, target_part);

        IF min_distance IS NULL OR word_distance < min_distance THEN
            min_distance := word_distance;
        END IF;
    END LOOP;

    RETURN LEAST(source_whole_distance, min_distance);
END;
$$ LANGUAGE plpgsql;

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20261006120000_RestoreLevenshteinFunction', '8.0.3')
ON CONFLICT ("MigrationId") DO NOTHING;

COMMIT;
