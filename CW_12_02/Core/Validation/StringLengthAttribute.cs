namespace StudentMetaFramework.Core.Validation;

public sealed class StringLengthAttribute : ValidationAttribute
{
    public StringLengthAttribute(int maxLength)
    {
        MaxLength = maxLength;
    }

    public int MinLength { get; init; }

    public int MaxLength { get; }

    public override string? Validate(string propertyName, object? value)
    {
        if (value is not string text || text.Length == 0)
        {
            return null;
        }

        return text.Length < MinLength || text.Length > MaxLength
            ? $"{propertyName} має бути від {MinLength} до {MaxLength} символів"
            : null;
    }
}
