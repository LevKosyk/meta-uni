using System.Text;
using StudentMetaFramework.Core.Mapping;
using StudentMetaFramework.Core.Models;
using StudentMetaFramework.Core.Validation;
using StudentMetaFramework.Tests;

namespace StudentMetaFramework.App;

public static class Program
{
    public static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;

        RunTests();

        var dataDir = Path.Combine(AppContext.BaseDirectory, "data");
        Directory.CreateDirectory(dataDir);

        var usersPath = Path.Combine(dataDir, "users.csv");
        var productsPath = Path.Combine(dataDir, "products.csv");

        CreateIfMissing(usersPath,
            "login,email,age",
            "anna,anna@mail.com,25",
            "bo,bo@mail.com,17",
            ",empty@mail.com,30",
            "max,max@mail.com,150",
            "olga,olga@mail.com,abc",
            "ivan,,40");

        CreateIfMissing(productsPath,
            "name,price,quantity,category",
            "Ноутбук,25000,5,Електроніка",
            "Мишка,-10,20,Електроніка",
            "X,100,3,",
            "Стіл,3500,many,Меблі",
            "Крісло,4200.50,2,Меблі");

        Process<User>("Користувачі", usersPath);
        Process<Product>("Товари", productsPath);
    }

    private static void RunTests()
    {
        var results = TestRunner.RunAll(typeof(TestRunner).Assembly);

        Console.WriteLine("Тести");
        foreach (var result in results)
        {
            Console.WriteLine(result.Passed ? $"ок {result.Name}" : $"впав {result.Name} - {result.Error}");
        }

        Console.WriteLine($"Пройшло {results.Count(r => r.Passed)}, впало {results.Count(r => !r.Passed)}");
        Console.WriteLine();
    }

    private static void CreateIfMissing(string path, params string[] lines)
    {
        if (File.Exists(path))
        {
            return;
        }

        File.WriteAllLines(path, lines, new UTF8Encoding(false));
        Console.WriteLine($"Файлу {Path.GetFileName(path)} не було, створив демо файл");
    }

    private static void Process<T>(string title, string path) where T : new()
    {
        Console.WriteLine(title);

        ImportResult<T> import;
        try
        {
            import = new CsvMapper<T>().Import(path);
        }
        catch (CsvMappingException ex)
        {
            Console.WriteLine(ex.Message);
            Console.WriteLine();
            return;
        }
        catch (IOException ex)
        {
            Console.WriteLine($"Не вдалося прочитати файл. {ex.Message}");
            Console.WriteLine();
            return;
        }

        Console.WriteLine($"Імпортовано {import.Items.Count}");

        foreach (var error in import.Errors)
        {
            Console.WriteLine($"Не імпортовано, {error}");
        }

        var valid = 0;
        foreach (var item in import.Items)
        {
            var errors = Validator.Validate(item!);
            if (errors.Count == 0)
            {
                valid++;
                Console.WriteLine($"{item} - ок");
                continue;
            }

            Console.WriteLine($"{item} - є помилки");
            foreach (var error in errors)
            {
                Console.WriteLine($"   {error}");
            }
        }

        Console.WriteLine($"Валідних {valid}, з помилками {import.Items.Count - valid}");
        Console.WriteLine();
    }
}
