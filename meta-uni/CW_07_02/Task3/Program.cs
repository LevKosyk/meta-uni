using System.Globalization;
using System.Text;

namespace Task3.BinaryStore;

public static class Program
{
    private static readonly Product[] SampleProducts =
    [
        new(1, 25.50, "Кава зернова", "Напої"),
        new(2, 3.99, "Олівець", "Канцелярія"),
        new(3, 1299.00, "Навушники", "Електроніка"),
    ];

    public static int Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;

        if (args.Length == 0)
        {
            PrintUsage();
            return 1;
        }

        var command = args[0];
        var options = args.Skip(1).Where(a => a.StartsWith("--", StringComparison.Ordinal)).ToArray();
        var positional = args.Skip(1).Where(a => !a.StartsWith("--", StringComparison.Ordinal)).ToArray();
        var path = positional.FirstOrDefault() ?? Path.Combine(AppContext.BaseDirectory, "products.bin");

        try
        {
            return command switch
            {
                "write" => WriteCommand(path, options),
                "read" => ReadCommand(path),
                _ => UnknownCommand(command),
            };
        }
        catch (FileNotFoundException)
        {
            Console.WriteLine($"Помилка: файл '{path}' не знайдено.");
        }
        catch (DirectoryNotFoundException)
        {
            Console.WriteLine($"Помилка: теку для '{path}' не знайдено.");
        }
        catch (InvalidDataException ex)
        {
            Console.WriteLine($"Помилка формату: {ex.Message}");
        }
        catch (EndOfStreamException)
        {
            Console.WriteLine("Помилка формату: файл обрізаний або пошкоджений.");
        }
        catch (UnauthorizedAccessException)
        {
            Console.WriteLine($"Помилка: недостатньо прав для '{path}'.");
        }
        catch (IOException ex)
        {
            Console.WriteLine($"Помилка вводу-виводу: {ex.Message}");
        }

        return 1;
    }

    private static int WriteCommand(string path, string[] options)
    {
        var version = ProductStore.V2;
        foreach (var option in options)
        {
            switch (option)
            {
                case "--v1":
                    version = ProductStore.V1;
                    break;
                case "--v2":
                    version = ProductStore.V2;
                    break;
                default:
                    Console.WriteLine($"Помилка: невідомий параметр '{option}'.");
                    return 1;
            }
        }

        if (File.Exists(path))
        {
            Console.WriteLine($"Увага: файл '{path}' буде перезаписано.");
        }

        ProductStore.Write(path, SampleProducts, version);
        Console.WriteLine($"Записано {SampleProducts.Length} товари у форматі v{version}: {Path.GetFullPath(path)} ({new FileInfo(path).Length} байт)");
        return 0;
    }

    private static int ReadCommand(string path)
    {
        var (version, products) = ProductStore.Read(path);

        Console.WriteLine($"Файл: {Path.GetFullPath(path)}, версія v{version}, товарів: {products.Count}");
        foreach (var product in products)
        {
            var price = product.Price.ToString("F2", CultureInfo.InvariantCulture);
            Console.WriteLine($"#{product.Id} {product.Name} {price} Category={product.Category ?? "(none)"}");
        }

        return 0;
    }

    private static int UnknownCommand(string command)
    {
        Console.WriteLine($"Помилка: невідома команда '{command}'.");
        PrintUsage();
        return 1;
    }

    private static void PrintUsage()
    {
        Console.WriteLine("Використання:");
        Console.WriteLine("  dotnet run --project Task3 -- write [файл] [--v1|--v2]");
        Console.WriteLine("  dotnet run --project Task3 -- read [файл]");
    }
}
