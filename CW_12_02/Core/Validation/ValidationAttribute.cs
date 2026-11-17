namespace StudentMetaFramework.Core.Validation;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
public abstract class ValidationAttribute : Attribute
{
    public abstract string? Validate(string propertyName, object? value);
}
