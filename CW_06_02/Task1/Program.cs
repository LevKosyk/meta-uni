namespace Task1.ConfigParser;

public static class Program
{
    public static int Main(string[] args)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        Console.WriteLine("=== ParseSetting ===");
        DemoParseSetting("host = localhost");
        DemoParseSetting("connection=Server=db;Database=shop");
        DemoParseSetting("timeout 30");
        DemoParseSetting("= no-key");
        DemoParseSetting("   ");
        DemoParseSetting(null);

        var samplesDirectory = Path.Combine(AppContext.BaseDirectory, "samples");
        var paths = args.Length > 0
            ? args
            : [
                Path.Combine(samplesDirectory, "app.conf"),
                Path.Combine(samplesDirectory, "broken.conf"),
                Path.Combine(samplesDirectory, "missing.conf"),
            ];

        var exitCode = 0;
        foreach (var path in paths)
        {
            Console.WriteLine();
            Console.WriteLine($"=== ParseFile: {Path.GetFileName(path)} ===");

            if (!DemoParseFile(path))
            {
                exitCode = 1;
            }
        }

        return exitCode;
    }

    private static void DemoParseSetting(string? input)
    {
        try
        {
            var (key, value) = ConfigParser.ParseSetting(input);
            Console.WriteLine($"OK    \"{input}\" -> key: '{key}', value: '{value}'");
        }
        catch (ArgumentNullException ex)
        {
            Console.WriteLine($"{nameof(ArgumentNullException)}: {ex.Message}");
        }
        catch (FormatException ex)
        {
            Console.WriteLine($"{nameof(FormatException)}: {ex.Message}");
        }
    }

    private static bool DemoParseFile(string path)
    {
        try
        {
            var settings = ConfigParser.ParseFile(path);
            foreach (var (key, value) in settings)
            {
                Console.WriteLine($"  {key} = {value}");
            }

            Console.WriteLine($"Прочитано налаштувань: {settings.Count}");
            return true;
        }
        catch (ConfigFormatException ex)
        {
            Console.WriteLine($"{nameof(ConfigFormatException)} (рядок {ex.LineNumber}): {ex.Message}");
            Console.WriteLine($"  InnerException -> {ex.InnerException?.GetType().Name}: {ex.InnerException?.Message}");
            return false;
        }
        catch (FileNotFoundException ex)
        {
            Console.WriteLine($"{nameof(FileNotFoundException)}: файл '{ex.FileName}' не знайдено.");
            return false;
        }
        finally
        {
            Console.WriteLine($"Обробку '{Path.GetFileName(path)}' завершено.");
        }
    }
}
