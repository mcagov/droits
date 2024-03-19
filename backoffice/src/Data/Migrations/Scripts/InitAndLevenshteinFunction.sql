CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory" (
    "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL,
    CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId")
);



CREATE OR REPLACE FUNCTION get_smallest_levenshtein_distance(source_text text, target_text text)
RETURNS INT AS $$
DECLARE
    source_whole_distance INT;
    min_distance INT := NULL;
    target_parts text[];
    word_distance INT;
    target_part text;
BEGIN
    -- Calculate Levenshtein distance for the entire source and target texts
    source_whole_distance := LEVENSHTEIN(source_text, target_text);

    -- Split the target text into an array of parts
    target_parts := string_to_array(target_text, ' ');

    -- Initialize min_distance with null
    min_distance := NULL;

    -- Iterate over each word in the target text
    FOR i IN 1..array_length(target_parts, 1) LOOP
        target_part := target_parts[i];

        -- Calculate Levenshtein distance between the source text and the current word
        word_distance := LEVENSHTEIN(source_text, target_part);
        
        -- Update min_distance if it's null or the new word_distance is smaller
        IF min_distance IS NULL OR word_distance < min_distance THEN
            min_distance := word_distance;
        END IF;
    END LOOP;

    -- Return the minimum of source_whole_distance and min_distance
    RETURN LEAST(source_whole_distance, min_distance);
END;
$$ LANGUAGE plpgsql;
            

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20240319154554_AddLevenshteinFunction', '8.0.3');

