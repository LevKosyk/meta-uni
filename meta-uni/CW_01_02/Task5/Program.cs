using System.Text;

namespace Task5.MiniLog;

public static class Program
{
    public static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;
        Console.WriteLine("=== MiniLog demo ===");

        MiniLog.TraceIfEnabled("Початок виконання");
        DoWork();
        new Worker().Run();

        Console.WriteLine("=== Кінець ===");
    }

    private static void DoWork()
    {
        MiniLog.TraceIfEnabled("Виконуємо DoWork");
    }
}
