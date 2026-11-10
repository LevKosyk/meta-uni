using System.Reflection;
using System.Text;

namespace Task5.Plugins;

// Тут шукаємо всі плагіни в збірці і запускаємо їх
public static class Program
{
    public static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;

        var pluginTypes = Assembly.GetExecutingAssembly()
            .GetTypes()
            .Where(t => typeof(IPlugin).IsAssignableFrom(t) && t is { IsClass: true, IsAbstract: false })
            .OrderBy(t => t.Name)
            .ToList();

        Console.WriteLine($"Плагінів {pluginTypes.Count}");

        foreach (var type in pluginTypes)
        {
            Console.WriteLine();
            Console.WriteLine(type.Name);

            if (Activator.CreateInstance(type) is IPlugin plugin)
            {
                plugin.Execute();
            }
        }
    }
}
