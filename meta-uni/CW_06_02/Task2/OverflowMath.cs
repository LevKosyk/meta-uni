namespace Task2.Overflow;

public static class OverflowMath
{
    public static int AddChecked(int a, int b) => checked(a + b);

    public static int AddWrapped(int a, int b) => unchecked(a + b);

    public static int AddDefault(int a, int b) => a + b;

    public static int SumAll(int[] values, bool safe)
    {
        ArgumentNullException.ThrowIfNull(values);

        return safe ? SumChecked(values) : SumUnchecked(values);
    }

    private static int SumChecked(int[] values)
    {
        var sum = 0;
        checked
        {
            foreach (var value in values)
            {
                sum += value;
            }
        }

        return sum;
    }

    private static int SumUnchecked(int[] values)
    {
        var sum = 0;
        unchecked
        {
            foreach (var value in values)
            {
                sum += value;
            }
        }

        return sum;
    }
}
