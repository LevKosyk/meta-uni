using System.Text;

namespace Task2.ConfigUpdater;

public static class Program
{
    public static int Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;

        var target = Path.Combine(AppContext.BaseDirectory, "appsettings.txt");
        string? template = null;
        var overrides = new List<string>();

        for (var i = 0; i < args.Length; i++)
        {
            switch (args[i])
            {
                case "--file" when i + 1 < args.Length:
                    target = args[++i];
                    break;
                case "--template" when i + 1 < args.Length:
                    template = args[++i];
                    break;
                default:
                    overrides.Add(args[i]);
                    break;
            }
        }

        try
        {
            var settings = template is null
                ? new Dictionary<string, string>
                {
                    ["AppName"] = "Inventory",
                    ["LogLevel"] = "Information",
                    ["MaxConnections"] = "10",
                }
                : ConfigWriter.ReadTemplate(template);

            foreach (var pair in overrides)
            {
                if (!ConfigWriter.TryParsePair(pair, out var key, out var value))
                {
                    Console.WriteLine($"Помилка: аргумент '{pair}' не має формату KEY=VALUE.");
                    return 1;
                }

                settings[key] = value;
            }

            settings["UpdatedUtc"] = DateTimeOffset.UtcNow.ToString("O");

            Console.WriteLine("Новий вміст:");
            foreach (var (key, value) in settings)
            {
                Console.WriteLine($"  {key}={value}");
            }

            var result = ConfigWriter.WriteAtomically(Path.GetFullPath(target), settings);

            Console.WriteLine();
            Console.WriteLine($"Файл оновлено: {result.TargetPath}");
            Console.WriteLine(result.BackupPath is null
                ? "Резервна копія: не потрібна, файл створено вперше"
                : $"Резервна копія: {result.BackupPath}");
            Console.WriteLine("Результат: OK");
            return 0;
        }
        catch (FileNotFoundException ex)
        {
            Console.WriteLine($"Помилка: файл '{ex.FileName}' не знайдено.");
        }
        catch (DirectoryNotFoundException)
        {
            Console.WriteLine($"Помилка: теку для '{target}' не знайдено.");
        }
        catch (FormatException ex)
        {
            Console.WriteLine($"Помилка шаблону: {ex.Message}");
        }
        catch (UnauthorizedAccessException)
        {
            Console.WriteLine($"Помилка: недостатньо прав для запису '{target}'.");
        }
        catch (PathTooLongException)
        {
            Console.WriteLine("Помилка: занадто довгий шлях.");
        }
        catch (IOException ex)
        {
            Console.WriteLine($"Помилка: файл зайнятий або недоступний. {ex.Message}");
        }

        Console.WriteLine("Результат: FAILED, попередня версія файлу не змінена");
        return 1;
    }
}
