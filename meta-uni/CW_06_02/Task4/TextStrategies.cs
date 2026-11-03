namespace Task4.Strategies;

public static class TextStrategies
{
    public static string Transform(string s, Func<string, string> strategy)
    {
        ArgumentNullException.ThrowIfNull(s);
        ArgumentNullException.ThrowIfNull(strategy);

        return strategy(s);
    }

    public static string TrimToUpper(string s) => s.Trim().ToUpperInvariant();

    public static string MaskDigits(string s) => new(s.Select(c => char.IsDigit(c) ? '*' : c).ToArray());
}
