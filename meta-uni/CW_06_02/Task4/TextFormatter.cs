namespace Task4.Strategies;

public class TextFormatter
{
    private Func<string, string> _strategy = s => s;

    public Func<string, string> Strategy
    {
        get => _strategy;
        set => _strategy = value ?? throw new ArgumentNullException(nameof(value), $"{nameof(Strategy)} не може бути null.");
    }

    public IReadOnlyList<string> FormatAll(IEnumerable<string> items)
    {
        ArgumentNullException.ThrowIfNull(items);

        var strategy = _strategy;
        return items.Select(strategy).ToList();
    }
}
