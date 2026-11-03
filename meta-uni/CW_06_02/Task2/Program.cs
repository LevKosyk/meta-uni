using System.Reflection;

namespace Task2.Overflow;

public static class Program
{
    private const string SafeFlag = "--safe";
    private const string FastFlag = "--fast";

    public static int Main(string[] args)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        if (!TryParseArguments(args, out var safe, out var values, out var error))
        {
            Console.WriteLine($"Помилка: {error}");
            PrintUsage();
            return 1;
        }

        var configuration = typeof(Program).Assembly
            .GetCustomAttribute<AssemblyConfigurationAttribute>()?.Configuration ?? "невідома";
        Console.WriteLine($"Конфігурація збірки: {configuration}");
        Console.WriteLine();

        Console.WriteLine("=== AddChecked / AddWrapped / AddDefault ===");
        ShowAddition(int.MaxValue, 1);
        ShowAddition(int.MinValue, -1);
        ShowAddition(100, 200);

        Console.WriteLine();
        Console.WriteLine($"=== SumAll, режим {(safe ? SafeFlag : FastFlag)} ===");
        Console.WriteLine($"Значення: [{string.Join(", ", values)}]");

        try
        {
            var sum = OverflowMath.SumAll(values, safe);
            Console.WriteLine($"Сума: {sum}");
            return 0;
        }
        catch (OverflowException ex)
        {
            Console.WriteLine($"{nameof(OverflowException)}: {ex.Message}");
            Console.WriteLine($"Підказка: запустіть з {FastFlag}, щоб отримати wrap-результат.");
            return 2;
        }
    }

    private static void ShowAddition(int a, int b)
    {
        var checkedResult = Describe(() => OverflowMath.AddChecked(a, b));
        var wrappedResult = Describe(() => OverflowMath.AddWrapped(a, b));
        var defaultResult = Describe(() => OverflowMath.AddDefault(a, b));

        Console.WriteLine($"{a} + {b}: checked -> {checkedResult}; unchecked -> {wrappedResult}; default -> {defaultResult}");
    }

    private static string Describe(Func<int> operation)
    {
        try
        {
            return operation().ToString();
        }
        catch (OverflowException)
        {
            return nameof(OverflowException);
        }
    }

    private static bool TryParseArguments(string[] args, out bool safe, out int[] values, out string? error)
    {
        safe = true;
        values = [int.MaxValue, 1, 10];
        error = null;

        var flags = args.Where(IsFlag).ToArray();
        var numbers = args.Where(a => !IsFlag(a)).ToArray();

        if (flags.Length > 1)
        {
            error = $"вкажіть лише один режим: {SafeFlag} або {FastFlag}.";
            return false;
        }

        if (flags.Length == 1)
        {
            switch (flags[0])
            {
                case SafeFlag:
                    safe = true;
                    break;
                case FastFlag:
                    safe = false;
                    break;
                default:
                    error = $"невідомий параметр '{flags[0]}'.";
                    return false;
            }
        }

        if (numbers.Length == 0)
        {
            return true;
        }

        var parsed = new int[numbers.Length];
        for (var i = 0; i < numbers.Length; i++)
        {
            if (!int.TryParse(numbers[i], out parsed[i]))
            {
                error = $"'{numbers[i]}' не є цілим числом типу int.";
                return false;
            }
        }

        values = parsed;
        return true;
    }

    private static bool IsFlag(string arg) => arg.StartsWith("--", StringComparison.Ordinal);

    private static void PrintUsage()
    {
        Console.WriteLine($"Використання: dotnet run --project Task2.Overflow -- [{SafeFlag}|{FastFlag}] [числа...]");
        Console.WriteLine($"  {SafeFlag}  checked-підсумовування, при переповненні кидає OverflowException (за замовчуванням)");
        Console.WriteLine($"  {FastFlag}  unchecked-підсумовування, при переповненні значення «загортається»");
    }
}
