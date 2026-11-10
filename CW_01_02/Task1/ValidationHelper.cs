using System.ComponentModel.DataAnnotations;

namespace Task1.RegistrationValidation;

public static class ValidationHelper
{
    public static IReadOnlyList<string> Validate(object instance)
    {
        ArgumentNullException.ThrowIfNull(instance);

        var results = new List<ValidationResult>();
        var context = new ValidationContext(instance);

        if (Validator.TryValidateObject(instance, context, results, validateAllProperties: true))
        {
            return [];
        }

        return results
            .Select(r =>
            {
                var members = string.Join(", ", r.MemberNames);
                return members.Length > 0 ? $"[{members}]: {r.ErrorMessage}" : r.ErrorMessage ?? string.Empty;
            })
            .ToList()
            .AsReadOnly();
    }
}
