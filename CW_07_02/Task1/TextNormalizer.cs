using System.Text;

namespace Task1.TextNormalizer;

public record NormalizeResult(int LineCount, int RemovedSpaces, string? Bom);

public static class TextNormalizer
{
    private static readonly UTF8Encoding Utf8NoBom = new(encoderShouldEmitUTF8Identifier: false);

    public static NormalizeResult NormalizeFile(string sourcePath, string destinationPath)
    {
        var bom = DetectBom(sourcePath);
        var lineCount = 0;
        var removedSpaces = 0;

        using var reader = new StreamReader(sourcePath, Encoding.UTF8, detectEncodingFromByteOrderMarks: true);
        using var writer = new StreamWriter(destinationPath, append: false, Utf8NoBom) { NewLine = "\n" };

        string? line;
        while ((line = reader.ReadLine()) is not null)
        {
            var trimmed = line.TrimEnd(' ', '\t');
            removedSpaces += line.Length - trimmed.Length;
            lineCount++;
            writer.WriteLine(trimmed);
        }

        return new NormalizeResult(lineCount, removedSpaces, bom);
    }

    public static string? DetectBom(string path)
    {
        Span<byte> buffer = stackalloc byte[3];
        using var stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
        var read = stream.ReadAtLeast(buffer, buffer.Length, throwOnEndOfStream: false);

        return buffer[..read] switch
        {
            [0xEF, 0xBB, 0xBF] => "UTF-8",
            [0xFF, 0xFE, ..] => "UTF-16 LE",
            [0xFE, 0xFF, ..] => "UTF-16 BE",
            _ => null,
        };
    }
}
