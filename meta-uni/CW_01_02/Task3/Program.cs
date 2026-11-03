using System.Reflection;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace Task3.EfModel;

public static class Program
{
    public static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;

        var type = typeof(User);
        Console.WriteLine($"Сутність: {type.Name}");

        foreach (var index in type.GetCustomAttributes<IndexAttribute>())
        {
            Console.WriteLine($"  [Index({string.Join(", ", index.PropertyNames)}), IsUnique = {index.IsUnique}]");
        }

        foreach (var property in type.GetProperties())
        {
            var attributes = property.GetCustomAttributes()
                .Where(a => a.GetType().Namespace != "System.Runtime.CompilerServices")
                .Select(Describe)
                .ToArray();

            var nullable = new NullabilityInfoContext().Create(property).WriteState == NullabilityState.Nullable;
            var typeName = property.PropertyType.Name + (nullable ? "?" : string.Empty);

            Console.WriteLine($"  {property.Name,-12} {typeName,-9} {string.Join(" ", attributes)}");
        }
    }

    private static string Describe(Attribute attribute) => attribute switch
    {
        System.ComponentModel.DataAnnotations.MaxLengthAttribute a => $"[MaxLength({a.Length})]",
        PrecisionAttribute a => $"[Precision({a.Precision})]",
        _ => $"[{attribute.GetType().Name.Replace("Attribute", string.Empty)}]",
    };
}
