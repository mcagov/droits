using System.Text.Json;
using System.Text.Json.Serialization;

namespace Droits.Converters;

public class FlexibleNullableBooleanConverter : JsonConverter<bool?>
{
    public override bool? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        switch (reader.TokenType)
        {
            case JsonTokenType.String:
            {
                var str = reader.GetString();
                if (string.IsNullOrWhiteSpace(str))
                    return null;
                if (bool.TryParse(str, out var result))
                    return result;
                break;
            }
            case JsonTokenType.True:
                return true;
            case JsonTokenType.False:
                return false;
        }

        return null;
    }

    public override void Write(Utf8JsonWriter writer, bool? value, JsonSerializerOptions options)
    {
        if (value.HasValue) writer.WriteBooleanValue(value.Value);
        else writer.WriteNullValue();
    }
}