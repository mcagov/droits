using Microsoft.EntityFrameworkCore;

namespace Droits.Data;

public static class DatabaseFunctions
{
    public const string EnsureSmallestLevenshteinDistanceSql =
        """
        DO $do$
        BEGIN
            IF to_regprocedure('get_smallest_levenshtein_distance(text, text)') IS NULL THEN
                CREATE FUNCTION get_smallest_levenshtein_distance(source_text text, target_text text)
                RETURNS INT AS $fn$
                DECLARE
                    source_whole_distance INT;
                    min_distance INT := NULL;
                    target_parts text[];
                    word_distance INT;
                    target_part text;
                BEGIN
                    source_whole_distance := LEVENSHTEIN(source_text, target_text);
                    target_parts := string_to_array(target_text, ' ');

                    FOR i IN 1..array_length(target_parts, 1) LOOP
                        target_part := target_parts[i];
                        word_distance := LEVENSHTEIN(source_text, target_part);

                        IF min_distance IS NULL OR word_distance < min_distance THEN
                            min_distance := word_distance;
                        END IF;
                    END LOOP;

                    RETURN LEAST(source_whole_distance, min_distance);
                END;
                $fn$ LANGUAGE plpgsql;
            END IF;
        END
        $do$;
        """;

    public static void EnsureCreated(DroitsContext dbContext)
    {
        if ( !dbContext.Database.IsNpgsql() )
        {
            return;
        }

        dbContext.Database.ExecuteSqlRaw(EnsureSmallestLevenshteinDistanceSql);
    }
}
