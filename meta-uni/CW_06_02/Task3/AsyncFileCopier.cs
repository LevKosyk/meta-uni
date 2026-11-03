using System.Buffers;

namespace Task3.Resources;

public sealed class AsyncFileCopier : IAsyncDisposable
{
    private readonly SemaphoreSlim _gate = new(1, 1);
    private readonly byte[] _buffer;
    private int _disposed;

    public AsyncFileCopier(int bufferSize = 81920)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(bufferSize);

        _buffer = ArrayPool<byte>.Shared.Rent(bufferSize);
    }

    public async Task<long> CopyAsync(string src, string dst, CancellationToken ct = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(src);
        ArgumentException.ThrowIfNullOrWhiteSpace(dst);
        ObjectDisposedException.ThrowIf(_disposed != 0, this);

        if (!_gate.Wait(0))
        {
            throw new InvalidOperationException(
                $"{nameof(AsyncFileCopier)} вже виконує копіювання; дочекайтеся завершення попереднього виклику {nameof(CopyAsync)}.");
        }

        try
        {
            ObjectDisposedException.ThrowIf(_disposed != 0, this);

            await using var source = new FileStream(
                src, FileMode.Open, FileAccess.Read, FileShare.Read, _buffer.Length, useAsync: true);
            await using var destination = new FileStream(
                dst, FileMode.Create, FileAccess.Write, FileShare.None, _buffer.Length, useAsync: true);

            long copied = 0;
            int read;
            while ((read = await source.ReadAsync(_buffer, ct)) > 0)
            {
                await destination.WriteAsync(_buffer.AsMemory(0, read), ct);
                copied += read;
            }

            await destination.FlushAsync(ct);
            return copied;
        }
        catch (OperationCanceledException)
        {
            File.Delete(dst);
            throw;
        }
        finally
        {
            _gate.Release();
        }
    }

    public async ValueTask DisposeAsync()
    {
        if (Interlocked.Exchange(ref _disposed, 1) != 0)
        {
            return;
        }

        await _gate.WaitAsync();
        ArrayPool<byte>.Shared.Return(_buffer);
        _gate.Dispose();
    }
}
