using System.Text;
using System.Text.Json;

namespace Task2.JsonContract;

public static class Program
{
    private static readonly JsonSerializerOptions Options = new() { WriteIndented = true };

    public static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;

        const string jsonA = """
            {
              "order_id": 101,
              "status": "new"
            }
            """;

        const string jsonB = """
            {
              "order_id": 202,
              "status": "paid",
              "tracking_code": "ZX-42",
              "priority": 3
            }
            """;

        Console.WriteLine("=== JSON A (мінімальний) ===");
        Console.WriteLine(jsonA);
        var dtoA = JsonSerializer.Deserialize<OrderDto>(jsonA, Options)!;
        DumpDto(dtoA);

        Console.WriteLine();
        Console.WriteLine("=== JSON B (розширений) ===");
        Console.WriteLine(jsonB);
        var dtoB = JsonSerializer.Deserialize<OrderDto>(jsonB, Options)!;
        DumpDto(dtoB);

        Console.WriteLine();
        Console.WriteLine("=== Серіалізація A назад (Comment = null опущено) ===");
        Console.WriteLine(JsonSerializer.Serialize(dtoA, Options));

        Console.WriteLine();
        Console.WriteLine("=== Серіалізація B назад (Comment задано, status = shipped) ===");
        dtoB.Comment = "Deliver to back door";
        dtoB.Status = OrderStatus.Shipped;
        Console.WriteLine(JsonSerializer.Serialize(dtoB, Options));

        Console.WriteLine();
        Console.WriteLine("=== Невідомий статус ===");
        try
        {
            JsonSerializer.Deserialize<OrderDto>("""{ "order_id": 1, "status": "lost" }""", Options);
        }
        catch (JsonException ex)
        {
            Console.WriteLine($"JsonException: {ex.Message}");
        }
    }

    private static void DumpDto(OrderDto dto)
    {
        Console.WriteLine("Після десеріалізації:");
        Console.WriteLine($"  Id: {dto.Id}");
        Console.WriteLine($"  Status: {dto.Status}");
        Console.WriteLine($"  Comment: {dto.Comment ?? "<null>"}");

        if (dto.Extra is { Count: > 0 })
        {
            Console.WriteLine("  Extra:");
            foreach (var (key, value) in dto.Extra)
            {
                Console.WriteLine($"    - {key}: {value}");
            }
        }
        else
        {
            Console.WriteLine("  Extra: <empty>");
        }
    }
}
