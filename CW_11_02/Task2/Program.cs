using System.Reflection;
using System.Text;

namespace Task2.DynamicInvoke;

// Тут викликаємо метод по його назві через рефлексію
public static class Program
{
    private const int A = 5;
    private const int B = 10;

    public static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;

        var operations = new MathOperations();
        var type = typeof(MathOperations);

        var add = type.GetMethod(nameof(MathOperations.Add))!;
        Console.WriteLine($"Add = {add.Invoke(operations, [A, B])}");

        Console.WriteLine();
        Console.Write("Введіть метод (Add, Sub, Mul, Div) ");
        var name = Console.ReadLine()?.Trim();

        if (string.IsNullOrEmpty(name))
        {
            Console.WriteLine("Нічого не введено");
            return;
        }

        var method = type.GetMethod(
            name,
            BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase | BindingFlags.DeclaredOnly,
            [typeof(int), typeof(int)]);

        if (method is null)
        {
            Console.WriteLine($"Немає методу {name}");
            return;
        }

        try
        {
            var result = method.Invoke(operations, [A, B]);
            Console.WriteLine($"{method.Name} = {result}");
        }
        catch (TargetInvocationException ex)
        {
            Console.WriteLine($"Помилка {ex.InnerException?.Message}");
        }
    }
}
