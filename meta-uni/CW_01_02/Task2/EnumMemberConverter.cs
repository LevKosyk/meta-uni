using System.Reflection;
using System.Runtime.Serialization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Task2.JsonContract;

public sealed class EnumMemberConverter<TEnum> : JsonConverter<TEnum>
    where TEnum : struct, Enum
{
    private static readonly Dictionary<TEnum, string> ToText = Enum.GetValues<TEnum>()
        .ToDictionary(value => value, GetMemberValue);

    private static readonly Dictionary<string, TEnum> FromText = ToText
        .ToDictionary(pair => pair.Value, pair => pair.Key, StringComparer.Ordinal);

    public override TEnum Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.String)
        {
            throw new JsonException($"{typeof(TEnum).Name}: очікується рядок, отримано {reader.TokenType}.");
        }

        var text = reader.GetString()!;
        return FromText.TryGetValue(text, out var value)
            ? value
            : throw new JsonException($"{typeof(TEnum).Name}: невідоме значення \"{text}\", допустимі: {string.Join(", ", FromText.Keys)}.");
    }

    public override void Write(Utf8JsonWriter writer, TEnum value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(ToText[value]);
    }

    private static string GetMemberValue(TEnum value)
    {
        var field = typeof(TEnum).GetField(value.ToString())!;
        return field.GetCustomAttribute<EnumMemberAttribute>()?.Value ?? value.ToString();
    }
}
