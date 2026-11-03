using System.Runtime.CompilerServices;

namespace Task1.ConfigParser;

public static class ConfigParser
{
    public static (string Key, string Value) ParseSetting(
        string? line,
        [CallerArgumentExpression(nameof(line))] string? lineExpression = null)
    {
        if (string.IsNullOrWhiteSpace(line))
        {
            throw new ArgumentNullException(
                nameof(line),
                $"Аргумент '{lineExpression}' порожній або null; очікується формат key=value.");
        }

        var separatorIndex = line.IndexOf('=');
        if (separatorIndex < 0)
        {
            throw new FormatException(
                $"Аргумент '{lineExpression}' (параметр {nameof(line)}) має значення \"{line}\" без символу '='; очікується формат key=value.");
        }

        var key = line[..separatorIndex].Trim();
        var value = line[(separatorIndex + 1)..].Trim();

        if (key.Length == 0)
        {
            throw new FormatException(
                $"Аргумент '{lineExpression}' (параметр {nameof(line)}) має значення \"{line}\" з порожнім ключем; очікується формат key=value.");
        }

        return (key, value);
    }

    public static Dictionary<string, string> ParseFile(string path)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(path);

        var settings = new Dictionary<string, string>();
        var lineNumber = 0;

        foreach (var rawLine in File.ReadLines(path))
        {
            lineNumber++;
            var line = rawLine.Trim();

            if (line.Length == 0 || line.StartsWith('#'))
            {
                continue;
            }

            try
            {
                var (key, value) = ParseSetting(line);
                settings[key] = value;
            }
            catch (Exception ex) when (ex is FormatException or ArgumentException)
            {
                throw new ConfigFormatException(path, lineNumber, rawLine, ex);
            }
        }

        return settings;
    }
}
