using System.Text;

namespace Task4.DynamicCreation;

// Тут створюємо Product через Activator і заповнюємо через PropertyInfo
public static class Program
{
    public static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;

        var type = typeof(Product);
        var product = Activator.CreateInstance(type)
            ?? throw new InvalidOperationException($"Не створився {type.Name}");

        Console.WriteLine("Після створення");
        Print(product);

        SetProperty(product, nameof(Product.Id), 1);
        SetProperty(product, nameof(Product.Name), "Laptop");
        SetProperty(product, nameof(Product.Price), 15000m);

        Console.WriteLine();
        Console.WriteLine("Після заповнення");
        Print(product);
    }

    private static void SetProperty(object target, string name, object value)
    {
        var property = target.GetType().GetProperty(name)
            ?? throw new MissingMemberException(target.GetType().Name, name);

        property.SetValue(target, value);
    }

    private static void Print(object obj)
    {
        foreach (var property in obj.GetType().GetProperties())
        {
            Console.WriteLine($"{property.Name} = {property.GetValue(obj)}");
        }
    }
}
