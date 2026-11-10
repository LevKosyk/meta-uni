using System.Text.Json.Serialization;

namespace Task4.JsonProfiles;

public class UserProfile
{
    public const string DefaultPhone = "не вказано";

    private DateTimeOffset _registeredUtc;

    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("full_name")]
    public string FullName { get; set; } = string.Empty;

    [JsonPropertyName("email")]
    public string Email { get; set; } = string.Empty;

    [JsonPropertyName("registered_utc")]
    public DateTimeOffset RegisteredUtc
    {
        get => _registeredUtc;
        set => _registeredUtc = value.ToUniversalTime();
    }

    [JsonPropertyName("phone")]
    public string Phone { get; set; } = DefaultPhone;

    [JsonIgnore]
    public bool IsInternal { get; set; }
}
