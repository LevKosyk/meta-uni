using System.Text;

namespace Task1.TypeInspector;

// Тут через рефлексію дивимось, які властивості і методи є в Student
public static class Program
{
    public static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;

        var student = new Student { Name = "Олена", Age = 19, Group = "КН-21" };
        student.PrintInfo();

        var type = student.GetType();

        Console.WriteLine();
        Console.WriteLine("Властивості");
        foreach (var property in type.GetProperties())
        {
            Console.WriteLine($"{property.Name} - {property.PropertyType.Name}");
        }

        Console.WriteLine();
        Console.WriteLine("Методи");
        foreach (var method in type.GetMethods())
        {
            var parameters = string.Join(", ", method.GetParameters().Select(p => $"{p.ParameterType.Name} {p.Name}"));
            Console.WriteLine($"{method.Name}({parameters}) з {method.DeclaringType?.Name}");
        }
    }
}
