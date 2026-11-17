using System.Globalization;
using System.Reflection;

namespace StudentMetaFramework.Core.Mapping;

public class CsvMapper<T> where T : new()
{
    private const char Separator = ',';

    public ImportResult<T> Import(string path)
    {
        var lines = File.ReadAllLines(path);
        if (lines.Length == 0)
        {
            throw new CsvMappingException($"Файл {Path.GetFileName(path)} порожній");
        }

        var header = lines[0].Split(Separator);
        var columns = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
        for (var i = 0; i < header.Length; i++)
        {
            columns[header[i].Trim()] = i;
        }

        var mapped = GetMappedProperties();
        foreach (var (column, _) in mapped)
        {
            if (!columns.ContainsKey(column))
            {
                throw new CsvMappingException($"У файлі {Path.GetFileName(path)} немає колонки {column}");
            }
        }

        var result = new ImportResult<T>();
        for (var i = 1; i < lines.Length; i++)
        {
            if (string.IsNullOrWhiteSpace(lines[i]))
            {
                continue;
            }

            var values = lines[i].Split(Separator);
            var item = new T();
            var rowErrors = new List<string>();

            foreach (var (column, property) in mapped)
            {
                var index = columns[column];
                var raw = index < values.Length ? values[index].Trim() : string.Empty;

                try
                {
                    property.SetValue(item, ConvertValue(raw, property.PropertyType));
                }
                catch (Exception ex) when (ex is FormatException or OverflowException or InvalidCastException)
                {
                    rowErrors.Add($"рядок {i + 1}, {column} = '{raw}' не підходить для {property.PropertyType.Name}");
                }
            }

            if (rowErrors.Count == 0)
            {
                result.Items.Add(item);
            }
            else
            {
                result.Errors.AddRange(rowErrors);
            }
        }

        return result;
    }

    private static List<(string Column, PropertyInfo Property)> GetMappedProperties()
    {
        var result = new List<(string, PropertyInfo)>();

        foreach (var property in typeof(T).GetProperties())
        {
            if (!property.CanWrite || property.GetCustomAttribute<CsvIgnoreAttribute>() != null)
            {
                continue;
            }

            var column = property.GetCustomAttribute<CsvColumnAttribute>();
            if (column != null)
            {
                result.Add((column.Name, property));
            }
        }

        return result;
    }

    private static object? ConvertValue(string raw, Type type)
    {
        if (type == typeof(string))
        {
            return raw;
        }

        var target = Nullable.GetUnderlyingType(type) ?? type;
        if (raw.Length == 0)
        {
            return type == target ? Activator.CreateInstance(type) : null;
        }

        return Convert.ChangeType(raw, target, CultureInfo.InvariantCulture);
    }
}
