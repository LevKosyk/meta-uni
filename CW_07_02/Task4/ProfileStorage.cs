using System.Text.Encodings.Web;
using System.Text.Json;

namespace Task4.JsonProfiles;

public static class ProfileStorage
{
    private static readonly JsonSerializerOptions Options = new()
    {
        WriteIndented = true,
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
    };

    public static void Save(string path, UserProfile profile) => SaveCore(path, profile);

    public static void SaveList(string path, IReadOnlyList<UserProfile> profiles) => SaveCore(path, profiles);

    public static List<UserProfile> Load(string path)
    {
        using var stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
        using var document = JsonDocument.Parse(stream);

        return document.RootElement.ValueKind switch
        {
            JsonValueKind.Array => document.RootElement.Deserialize<List<UserProfile>>(Options) ?? [],
            JsonValueKind.Object => [document.RootElement.Deserialize<UserProfile>(Options)!],
            _ => throw new JsonException("очікується об'єкт профілю або масив профілів."),
        };
    }

    private static void SaveCore<T>(string path, T value)
    {
        using var stream = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None);
        JsonSerializer.Serialize(stream, value, Options);
    }
}
