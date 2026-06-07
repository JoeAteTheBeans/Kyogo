using System.Text.Json;
using System.Text.Json.Serialization;
using Kyogo.Domain.Vocabulary.Components;

namespace Kyogo.Infrastructure.Serialisation;

public sealed class ComponentJsonConverter : JsonConverter<IComponent>
{
    private const string TypeProperty = "$type";

    public override IComponent Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        using JsonDocument doc = JsonDocument.ParseValue(ref reader);
        string type = doc.RootElement.GetProperty(TypeProperty).GetString()!;
        string json = doc.RootElement.GetRawText();
        return type switch
        {
            nameof(KanjiComponent) => JsonSerializer.Deserialize<KanjiComponent>(json, options)!,
            nameof(KanaComponent) => JsonSerializer.Deserialize<KanaComponent>(json, options)!,
            nameof(JukujikunComponent) => JsonSerializer.Deserialize<JukujikunComponent>(json, options)!,
            _ => throw new JsonException($"Unknown component type {type}")
        };
    }

    public override void Write(Utf8JsonWriter writer, IComponent value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        writer.WriteString(TypeProperty, value.GetType().Name);
        switch (value)
        {
            case KanjiComponent k:
                writer.WriteString(nameof(k.Kanji), k.Kanji);
                writer.WriteString(nameof(k.KanaReading), k.KanaReading);
                break;
            case KanaComponent k:
                writer.WriteString(nameof(k.Kana), k.Kana);
                break;
            case JukujikunComponent j:
                writer.WriteString(nameof(j.KanjiCharacters), j.KanjiCharacters);
                writer.WriteString(nameof(j.KanaReading), j.KanaReading);
                break;
        }
        writer.WriteEndObject();
    }
}