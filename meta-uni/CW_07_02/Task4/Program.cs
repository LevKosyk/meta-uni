using System.Text;
using System.Text.Json;

namespace Task4.JsonProfiles;

public static class Program
{
    public static int Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;

        if (args.Length == 0)
        {
            PrintUsage();
            return 1;
        }

        var command = args[0];
        var isList = args.Contains("--list");
        var path = args.Skip(1).FirstOrDefault(a => !a.StartsWith("--", StringComparison.Ordinal))
            ?? Path.Combine(AppContext.BaseDirectory, isList ? "profiles.json" : "profile.json");

        try
        {
            return command switch
            {
                "save" => SaveCommand(path, isList),
                "load" => LoadCommand(path),
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
        catch (JsonException ex)
        {
            Console.WriteLine($"Помилка: некоректний JSON у '{path}'. {ex.Message}");
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

    private static int SaveCommand(string path, bool isList)
    {
        if (File.Exists(path))
        {
            Console.WriteLine($"Увага: файл '{path}' буде перезаписано.");
        }

        var kyivTime = new DateTimeOffset(2026, 10, 1, 9, 30, 0, TimeSpan.FromHours(3));
        UserProfile[] profiles =
        [
            new() { Id = 1, FullName = "Олена Коваль", Email = "olena@example.com", RegisteredUtc = kyivTime, Phone = "+380671234567" },
            new() { Id = 2, FullName = "Іван Петренко", Email = "ivan@example.com", RegisteredUtc = DateTimeOffset.UtcNow, IsInternal = true },
        ];

        if (isList)
        {
            ProfileStorage.SaveList(path, profiles);
            Console.WriteLine($"Збережено список із {profiles.Length} профілів: {Path.GetFullPath(path)}");
        }
        else
        {
            ProfileStorage.Save(path, profiles[0]);
            Console.WriteLine($"Збережено профіль: {Path.GetFullPath(path)}");
        }

        Console.WriteLine(File.ReadAllText(path));
        return 0;
    }

    private static int LoadCommand(string path)
    {
        var profiles = ProfileStorage.Load(path);

        Console.WriteLine($"Завантажено профілів: {profiles.Count} з {Path.GetFullPath(path)}");
        foreach (var profile in profiles)
        {
            Console.WriteLine($"#{profile.Id} {profile.FullName} <{profile.Email}>");
            Console.WriteLine($"   registered: {profile.RegisteredUtc:O}");
            Console.WriteLine($"   phone:      {profile.Phone}");
            Console.WriteLine($"   internal:   {profile.IsInternal}");
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
        Console.WriteLine("  dotnet run --project Task4 -- save [файл] [--list]");
        Console.WriteLine("  dotnet run --project Task4 -- load [файл]");
    }
}
