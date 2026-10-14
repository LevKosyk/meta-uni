namespace Task1.ConfigParser;

public class ConfigFormatException : FormatException
{
    public ConfigFormatException()
    {
    }

    public ConfigFormatException(string message)
        : base(message)
    {
    }

    public ConfigFormatException(string message, Exception innerException)
        : base(message, innerException)
    {
    }

    public ConfigFormatException(string filePath, int lineNumber, string lineText, Exception innerException)
        : base($"Некоректний рядок {lineNumber} у файлі '{filePath}': \"{lineText}\".", innerException)
    {
        FilePath = filePath;
        LineNumber = lineNumber;
        LineText = lineText;
    }

    public string? FilePath { get; }

    public int LineNumber { get; }

    public string? LineText { get; }
}
