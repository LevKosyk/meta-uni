namespace StudentMetaFramework.Core.Validation;

public sealed class RangeAttribute : ValidationAttribute
{
    public RangeAttribute(double min, double max)
    {
        Min = min;
        Max = max;
    }

    public double Min { get; }

    public double Max { get; }

    public override string? Validate(string propertyName, object? value)
    {
        if (value is null)
        {
            return null;
        }

        var number = Convert.ToDouble(value);
        return number < Min || number > Max
            ? $"{propertyName} має бути від {Min} до {Max}, а зараз {number}"
            : null;
    }
}
