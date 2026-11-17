using System.Reflection;

namespace StudentMetaFramework.Core.Validation;

public static class Validator
{
    public static List<string> Validate(object obj)
    {
        ArgumentNullException.ThrowIfNull(obj);

        var errors = new List<string>();
        foreach (var property in obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
        {
            var value = property.GetValue(obj);
            foreach (var attribute in property.GetCustomAttributes<ValidationAttribute>())
            {
                var error = attribute.Validate(property.Name, value);
                if (error is not null)
                {
                    errors.Add(error);
                }
            }
        }

        return errors;
    }
}
