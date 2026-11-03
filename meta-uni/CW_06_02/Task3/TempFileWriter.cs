namespace Task3.Resources;

public sealed class TempFileWriter : IDisposable
{
    private StreamWriter? _writer;

    public TempFileWriter()
    {
        FilePath = Path.GetTempFileName();
        _writer = new StreamWriter(FilePath, append: false);
    }

    public string FilePath { get; }

    public bool IsDisposed => _writer is null;

    public void WriteLine(string text)
    {
        ArgumentNullException.ThrowIfNull(text);
        ObjectDisposedException.ThrowIf(_writer is null, this);

        _writer.WriteLine(text);
    }

    public void Dispose()
    {
        _writer?.Dispose();
        _writer = null;
    }
}
