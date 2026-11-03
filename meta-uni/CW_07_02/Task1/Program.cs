using System.Globalization;
using System.Text;

namespace Task1.TextNormalizer;

public static class Program
{
    private const long DefaultMaxSizeKb = 10 * 1024;

    public static int Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;

        if (!TryParseArguments(args, out var source, out var destination, out var maxSizeKb, out var error))
        {
            Console.WriteLine($"Помилка: {error}");
            Console.WriteLine("Використання: dotnet run --project Task1 -- [тека-джерело] [тека-призначення] [--max-kb N]");
            return 1;
        }

        try
        {
            return Run(source, destination, maxSizeKb * 1024);
        }
        catch (DirectoryNotFoundException)
        {
            Console.WriteLine($"Помилка: теку '{source}' не знайдено.");
        }
        catch (UnauthorizedAccessException ex)
        {
            Console.WriteLine($"Помилка: недостатньо прав. {ex.Message}");
        }
        catch (PathTooLongException)
        {
            Console.WriteLine("Помилка: занадто довгий шлях.");
        }
        catch (IOException ex)
        {
            Console.WriteLine($"Помилка вводу-виводу: {ex.Message}");
        }

        return 1;
    }

    private static int Run(string source, string destination, long maxSizeBytes)
    {
        source = Path.GetFullPath(source);
        destination = Path.GetFullPath(destination);

        if (!Directory.Exists(source))
        {
            throw new DirectoryNotFoundException();
        }

        if (Path.GetRelativePath(source, destination) is var relative && !relative.StartsWith("..") && !Path.IsPathRooted(relative))
        {
            Console.WriteLine("Помилка: тека призначення не може бути всередині теки-джерела.");
            return 1;
        }

        Console.WriteLine($"Джерело:     {source}");
        Console.WriteLine($"Призначення: {destination}");
        Console.WriteLine($"Поріг:       {maxSizeBytes / 1024} КБ");
        Console.WriteLine();

        var report = new List<string>();
        var skipped = 0;
        var failed = 0;

        foreach (var file in Directory.EnumerateFiles(source, "*.txt", SearchOption.AllDirectories))
        {
            var relativePath = Path.GetRelativePath(source, file);
            var size = new FileInfo(file).Length;

            if (size > maxSizeBytes)
            {
                Console.WriteLine($"ПРОПУСК  {relativePath}: {size / 1024.0:F1} КБ перевищує поріг");
                skipped++;
                continue;
            }

            var target = Path.Combine(destination, relativePath);
            Directory.CreateDirectory(Path.GetDirectoryName(target)!);

            if (File.Exists(target))
            {
                Console.WriteLine($"УВАГА    {relativePath}: файл у теці призначення буде перезаписано");
            }

            try
            {
                var result = TextNormalizer.NormalizeFile(file, target);
                var bom = result.Bom is null ? "без BOM" : $"BOM {result.Bom}";
                Console.WriteLine($"OK       {relativePath}: рядків {result.LineCount}, видалено пробілів {result.RemovedSpaces} ({bom})");
                report.Add(string.Join(';', relativePath, result.LineCount, result.RemovedSpaces));
            }
            catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
            {
                Console.WriteLine($"ПОМИЛКА  {relativePath}: {ex.Message}");
                failed++;
            }
        }

        Directory.CreateDirectory(destination);
        var reportPath = Path.Combine(destination, "report.csv");
        File.WriteAllLines(reportPath, ["path;lines;removed_spaces", .. report], new UTF8Encoding(false));

        Console.WriteLine();
        Console.WriteLine($"Оброблено: {report.Count}, пропущено: {skipped}, помилок: {failed}");
        Console.WriteLine($"Звіт: {reportPath}");
        return failed == 0 ? 0 : 2;
    }

    private static bool TryParseArguments(
        string[] args, out string source, out string destination, out long maxSizeKb, out string? error)
    {
        source = Path.Combine(AppContext.BaseDirectory, "samples");
        destination = Path.Combine(AppContext.BaseDirectory, "normalized");
        maxSizeKb = DefaultMaxSizeKb;
        error = null;

        var positional = new List<string>();
        for (var i = 0; i < args.Length; i++)
        {
            if (args[i] == "--max-kb")
            {
                if (i + 1 >= args.Length || !long.TryParse(args[++i], CultureInfo.InvariantCulture, out maxSizeKb) || maxSizeKb <= 0)
                {
                    error = "після --max-kb очікується додатне ціле число.";
                    return false;
                }
            }
            else if (args[i].StartsWith("--", StringComparison.Ordinal))
            {
                error = $"невідомий параметр '{args[i]}'.";
                return false;
            }
            else
            {
                positional.Add(args[i]);
            }
        }

        if (positional.Count > 2)
        {
            error = "забагато аргументів.";
            return false;
        }

        if (positional.Count > 0)
        {
            source = positional[0];
        }

        if (positional.Count > 1)
        {
            destination = positional[1];
        }

        return true;
    }
}
