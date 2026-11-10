using System.Reflection;

namespace Task3.AttributeValidation;

public static class Validator
{
    public static IReadOnlyList<string> Validate(object obj)
    {
        ArgumentNullException.ThrowIfNull(obj);

        var errors = new List<string>();
        foreach (var property in obj.GetType().GetProperties())
        {
            if (property.GetCustomAttribute<RequiredAttribute>() is null)
            {
                continue;
            }

            var value = property.GetValue(obj);
            if (value is null || value is string text && string.IsNullOrWhiteSpace(text))
            {
                errors.Add($"{property.Name} не заповнено");
            }
        }

        return errors;
    }
}
