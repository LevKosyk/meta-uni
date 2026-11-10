namespace Task4.Strategies;

public static class Program
{
    public static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        Console.WriteLine("=== Transform ===");
        const string sample = "  card 4111-1111, pin 0420  ";
        Console.WriteLine($"Вхід:            \"{sample}\"");
        Console.WriteLine($"TrimToUpper:     \"{TextStrategies.Transform(sample, TextStrategies.TrimToUpper)}\"");
        Console.WriteLine($"MaskDigits:      \"{TextStrategies.Transform(sample, TextStrategies.MaskDigits)}\"");
        Console.WriteLine($"Лямбда (обидві): \"{TextStrategies.Transform(sample, s => TextStrategies.MaskDigits(TextStrategies.TrimToUpper(s)))}\"");

        Console.WriteLine();
        Console.WriteLine("=== TextFormatter: зміна стратегії на льоту ===");

        string[] lines = ["  order 15 ", "Phone: +380 67 123 45 67", "  hello, world  "];
        var formatter = new TextFormatter();

        (string Name, Func<string, string> Strategy)[] strategies =
        [
            ("За замовчуванням (s => s)", formatter.Strategy),
            ("TrimToUpper (метод-група)", TextStrategies.TrimToUpper),
            ("MaskDigits (метод-група)", TextStrategies.MaskDigits),
            ("Лямбда: Trim + реверс", s => new string(s.Trim().Reverse().ToArray())),
            ("Лямбда: довжина рядка", s => $"{s.Trim()} ({s.Trim().Length} симв.)"),
        ];

        foreach (var (name, strategy) in strategies)
        {
            formatter.Strategy = strategy;

            Console.WriteLine($"-- {name}");
            foreach (var line in formatter.FormatAll(lines))
            {
                Console.WriteLine($"   \"{line}\"");
            }
        }

        try
        {
            formatter.Strategy = null!;
        }
        catch (ArgumentNullException ex)
        {
            Console.WriteLine();
            Console.WriteLine($"{nameof(ArgumentNullException)}: {ex.Message}");
        }
    }
}
