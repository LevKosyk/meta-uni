namespace Task5.InboxWatcher;

public sealed class Debouncer : IDisposable
{
    private readonly TimeSpan _quietPeriod;
    private readonly Action<string> _action;
    private readonly Dictionary<string, Timer> _timers = new(StringComparer.Ordinal);
    private readonly Lock _lock = new();

    public Debouncer(TimeSpan quietPeriod, Action<string> action)
    {
        _quietPeriod = quietPeriod;
        _action = action;
    }

    public void Trigger(string key)
    {
        lock (_lock)
        {
            if (_timers.TryGetValue(key, out var timer))
            {
                timer.Change(_quietPeriod, Timeout.InfiniteTimeSpan);
                return;
            }

            _timers[key] = new Timer(_ => Fire(key), null, _quietPeriod, Timeout.InfiniteTimeSpan);
        }
    }

    public void Dispose()
    {
        lock (_lock)
        {
            foreach (var timer in _timers.Values)
            {
                timer.Dispose();
            }

            _timers.Clear();
        }
    }

    private void Fire(string key)
    {
        lock (_lock)
        {
            if (_timers.Remove(key, out var timer))
            {
                timer.Dispose();
            }
        }

        _action(key);
    }
}
