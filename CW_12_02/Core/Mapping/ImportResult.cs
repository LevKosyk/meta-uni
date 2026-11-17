namespace StudentMetaFramework.Core.Mapping;

public class ImportResult<T>
{
    public List<T> Items { get; } = [];

    public List<string> Errors { get; } = [];
}
