namespace Task3.AttributeValidation;

[AttributeUsage(AttributeTargets.Property)]
public sealed class RequiredAttribute : Attribute
{
}
