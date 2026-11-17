namespace StudentMetaFramework.Core.Mapping;

[AttributeUsage(AttributeTargets.Property)]
public sealed class CsvColumnAttribute : Attribute
{
    public CsvColumnAttribute(string name)
    {
        Name = name;
    }

    public string Name { get; }
}
