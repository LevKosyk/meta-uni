using System.Text;

namespace Task2.ConfigUpdater;

public record UpdateResult(string TargetPath, string? BackupPath);

public static class ConfigWriter
{
    private static readonly UTF8Encoding Utf8NoBom = new(encoderShouldEmitUTF8Identifier: false);

    public static UpdateResult WriteAtomically(string targetPath, IReadOnlyDictionary<string, string> settings)
    {
        var tempPath = targetPath + ".tmp";
        var backupPath = targetPath + ".bak";

        try
        {
            WriteExclusive(tempPath, settings);

            if (File.Exists(targetPath))
            {
                File.Replace(tempPath, targetPath, backupPath);
                return new UpdateResult(targetPath, backupPath);
            }

            File.Move(tempPath, targetPath);
            return new UpdateResult(targetPath, null);
        }
        catch
        {
            if (File.Exists(tempPath))
            {
                File.Delete(tempPath);
            }

            throw;
        }
    }

    public static void WriteExclusive(string path, IReadOnlyDictionary<string, string> settings)
    {
        using var stream = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None);
        using var writer = new StreamWriter(stream, Utf8NoBom) { NewLine = "\n" };

        foreach (var (key, value) in settings)
        {
            writer.WriteLine($"{key}={value}");
        }

        writer.Flush();
        stream.Flush(flushToDisk: true);
    }

    public static Dictionary<string, string> ReadTemplate(string path)
    {
        var settings = new Dictionary<string, string>();
        var lineNumber = 0;

        foreach (var rawLine in File.ReadLines(path, Utf8NoBom))
        {
            lineNumber++;
            var line = rawLine.Trim();
            if (line.Length == 0 || line.StartsWith('#'))
            {
                continue;
            }

            if (!TryParsePair(line, out var key, out var value))
            {
                throw new FormatException($"рядок {lineNumber} у '{Path.GetFileName(path)}' не має формату KEY=VALUE: \"{rawLine}\".");
            }

            settings[key] = value;
        }

        return settings;
    }

    public static bool TryParsePair(string text, out string key, out string value)
    {
        var separator = text.IndexOf('=');
        key = separator > 0 ? text[..separator].Trim() : string.Empty;
        value = separator > 0 ? text[(separator + 1)..].Trim() : string.Empty;
        return key.Length > 0;
    }
}
