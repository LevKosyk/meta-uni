namespace Task5.InboxWatcher;

public sealed class InboxImporter : IDisposable
{
    private const int MaxAttempts = 3;
    private static readonly TimeSpan RetryDelay = TimeSpan.FromMilliseconds(500);

    private readonly FileSystemWatcher _watcher;
    private readonly Debouncer _debouncer;
    private readonly Lock _processLock = new();

    public InboxImporter(string rootDirectory, TimeSpan quietPeriod)
    {
        InboxDirectory = Path.Combine(rootDirectory, "inbox");
        ProcessedDirectory = Path.Combine(rootDirectory, "processed");
        ErrorLogPath = Path.Combine(rootDirectory, "errors.log");

        Directory.CreateDirectory(InboxDirectory);
        Directory.CreateDirectory(ProcessedDirectory);

        _debouncer = new Debouncer(quietPeriod, Process);
        _watcher = new FileSystemWatcher(InboxDirectory, "*.csv")
        {
            IncludeSubdirectories = true,
            NotifyFilter = NotifyFilters.FileName | NotifyFilters.LastWrite | NotifyFilters.Size,
        };

        _watcher.Created += OnChanged;
        _watcher.Changed += OnChanged;
        _watcher.Renamed += OnChanged;
        _watcher.Error += OnError;
    }

    public string InboxDirectory { get; }

    public string ProcessedDirectory { get; }

    public string ErrorLogPath { get; }

    public void Start() => _watcher.EnableRaisingEvents = true;

    public void Dispose()
    {
        _watcher.EnableRaisingEvents = false;
        _watcher.Created -= OnChanged;
        _watcher.Changed -= OnChanged;
        _watcher.Renamed -= OnChanged;
        _watcher.Error -= OnError;
        _watcher.Dispose();
        _debouncer.Dispose();
    }

    private void OnChanged(object sender, FileSystemEventArgs e)
    {
        if (e.FullPath.EndsWith(".csv", StringComparison.OrdinalIgnoreCase) && File.Exists(e.FullPath))
        {
            Log($"подія {e.ChangeType}: {Path.GetRelativePath(InboxDirectory, e.FullPath)}");
            _debouncer.Trigger(e.FullPath);
        }
    }

    private void OnError(object sender, ErrorEventArgs e)
    {
        WriteError(InboxDirectory, $"помилка спостерігача: {e.GetException().Message}");
    }

    private void Process(string sourcePath)
    {
        lock (_processLock)
        {
            if (!File.Exists(sourcePath))
            {
                return;
            }

            var relativeDirectory = Path.GetDirectoryName(Path.GetRelativePath(InboxDirectory, sourcePath)) ?? string.Empty;
            var targetDirectory = Path.Combine(ProcessedDirectory, relativeDirectory);

            for (var attempt = 1; attempt <= MaxAttempts; attempt++)
            {
                try
                {
                    Directory.CreateDirectory(targetDirectory);
                    var targetPath = BuildTargetPath(targetDirectory, sourcePath);

                    using (new FileStream(sourcePath, FileMode.Open, FileAccess.Read, FileShare.None))
                    {
                    }

                    File.Move(sourcePath, targetPath);
                    Log($"OK: {Path.GetRelativePath(InboxDirectory, sourcePath)} -> {Path.GetRelativePath(ProcessedDirectory, targetPath)}");
                    return;
                }
                catch (FileNotFoundException)
                {
                    return;
                }
                catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
                {
                    if (attempt == MaxAttempts)
                    {
                        WriteError(sourcePath, $"не вдалося перемістити після {MaxAttempts} спроб: {ex.Message}");
                        return;
                    }

                    Log($"файл зайнятий, спроба {attempt}/{MaxAttempts}: {Path.GetFileName(sourcePath)}");
                    Thread.Sleep(RetryDelay);
                }
            }
        }
    }

    private static string BuildTargetPath(string targetDirectory, string sourcePath)
    {
        var name = Path.GetFileNameWithoutExtension(sourcePath);
        var extension = Path.GetExtension(sourcePath);
        var date = DateTime.UtcNow.ToString("yyyyMMdd");

        var candidate = Path.Combine(targetDirectory, $"{name}-{date}{extension}");
        for (var index = 1; File.Exists(candidate); index++)
        {
            candidate = Path.Combine(targetDirectory, $"{name}-{date}-{index}{extension}");
        }

        return candidate;
    }

    private void WriteError(string path, string message)
    {
        var line = $"{DateTime.UtcNow:O};{path};{message}";
        File.AppendAllLines(ErrorLogPath, [line]);
        Log($"ПОМИЛКА: {message} (див. {Path.GetFileName(ErrorLogPath)})");
    }

    private static void Log(string message)
    {
        Console.WriteLine($"[{DateTime.Now:HH:mm:ss.fff}] {message}");
    }
}
