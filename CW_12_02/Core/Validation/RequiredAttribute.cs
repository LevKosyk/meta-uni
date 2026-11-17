namespace StudentMetaFramework.Core.Validation;

public sealed class RequiredAttribute : ValidationAttribute
{
    public override string? Validate(string propertyName, object? value)
    {
        return value is null || value is string text && string.IsNullOrWhiteSpace(text)
            ? $"{propertyName} обов'язкове"
            : null;
    }
}
