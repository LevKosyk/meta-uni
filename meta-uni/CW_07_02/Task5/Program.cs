using System.Text;

namespace Task5.InboxWatcher;

public static class Program
{
    public static int Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;

        var root = args.Length > 0 ? args[0] : Path.Combine(AppContext.BaseDirectory, "importer");

        try
        {
            using var importer = new InboxImporter(Path.GetFullPath(root), TimeSpan.FromMilliseconds(500));
            importer.Start();

            Console.WriteLine($"Спостерігаю за: {importer.InboxDirectory}");
            Console.WriteLine($"Оброблені:      {importer.ProcessedDirectory}");
            Console.WriteLine($"Журнал помилок: {importer.ErrorLogPath}");
            Console.WriteLine("Покладіть .csv у inbox/ (можна в підтеки). Enter — вихід.");

            Console.ReadLine();
            return 0;
        }
        catch (UnauthorizedAccessException)
        {
            Console.WriteLine($"Помилка: недостатньо прав для теки '{root}'.");
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
}
