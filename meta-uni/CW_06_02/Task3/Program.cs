namespace Task3.Resources;

public static class Program
{
    public static async Task Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        var tempFiles = new List<string>();
        try
        {
            DemoTempFileWriter(tempFiles);
            await DemoAsyncFileCopier(tempFiles);
        }
        finally
        {
            foreach (var path in tempFiles)
            {
                File.Delete(path);
            }

            Console.WriteLine();
            Console.WriteLine($"Тимчасові файли видалено: {tempFiles.Count}");
        }
    }

    private static void DemoTempFileWriter(List<string> tempFiles)
    {
        Console.WriteLine("=== TempFileWriter + using ===");

        string path;
        using (var writer = new TempFileWriter())
        {
            path = writer.FilePath;
            tempFiles.Add(path);

            writer.WriteLine("Перший рядок");
            writer.WriteLine("Другий рядок");
            writer.WriteLine($"Час запису: {DateTime.Now:HH:mm:ss}");
        }

        Console.WriteLine($"Файл: {path}");
        Console.WriteLine(File.ReadAllText(path).TrimEnd());

        var disposed = new TempFileWriter();
        tempFiles.Add(disposed.FilePath);
        disposed.Dispose();
        disposed.Dispose();
        Console.WriteLine($"Повторний Dispose() безпечний, IsDisposed = {disposed.IsDisposed}");

        try
        {
            disposed.WriteLine("Цей рядок не буде записано");
        }
        catch (ObjectDisposedException ex)
        {
            Console.WriteLine($"{nameof(ObjectDisposedException)}: {ex.Message}");
        }
    }

    private static async Task DemoAsyncFileCopier(List<string> tempFiles)
    {
        Console.WriteLine();
        Console.WriteLine("=== AsyncFileCopier + await using ===");

        var source = Path.GetTempFileName();
        var copy = source + ".copy";
        var cancelledCopy = source + ".cancelled";
        tempFiles.AddRange([source, copy, cancelledCopy]);

        await File.WriteAllBytesAsync(source, new byte[64 * 1024 * 1024]);

        await using (var copier = new AsyncFileCopier(bufferSize: 4096))
        {
            var copied = await copier.CopyAsync(source, copy);
            Console.WriteLine($"Скопійовано {copied:N0} байт -> {Path.GetFileName(copy)}");

            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(10));
            try
            {
                await copier.CopyAsync(source, cancelledCopy, cts.Token);
                Console.WriteLine("Копіювання встигло завершитися до скасування.");
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine($"Копіювання скасовано, частковий файл видалено: {!File.Exists(cancelledCopy)}");
            }
        }

        Console.WriteLine("DisposeAsync() виконано після виходу з блоку await using.");
    }
}
