using System.Runtime.Serialization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Task2.JsonContract;

public class OrderDto
{
    [JsonPropertyName("order_id")]
    public int Id { get; set; }

    [JsonPropertyName("status")]
    [JsonConverter(typeof(EnumMemberConverter<OrderStatus>))]
    public OrderStatus Status { get; set; }

    [JsonPropertyName("comment")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Comment { get; set; }

    [JsonExtensionData]
    public Dictionary<string, JsonElement>? Extra { get; set; }
}

public enum OrderStatus
{
    [EnumMember(Value = "new")]
    New,

    [EnumMember(Value = "paid")]
    Paid,

    [EnumMember(Value = "shipped")]
    Shipped,
}
