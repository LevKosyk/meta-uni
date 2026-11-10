using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Task5.MiniLog;

public static class MiniLog
{
    public static void Trace(
        string message,
        [CallerMemberName] string? member = null,
        [CallerFilePath] string? file = null,
        [CallerLineNumber] int line = 0)
    {
        var fileName = file is null ? "<unknown>" : Path.GetFileName(file);
        Console.WriteLine($"[TRACE] {fileName}:{line} {member}() - {message}");
    }

    [Conditional("TRACE")]
    public static void TraceIfEnabled(
        string message,
        [CallerMemberName] string? member = null,
        [CallerFilePath] string? file = null,
        [CallerLineNumber] int line = 0)
        => Trace(message, member, file, line);
}
