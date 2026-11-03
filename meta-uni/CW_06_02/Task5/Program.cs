namespace Task5.Events;

public static class Program
{
    public static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        var counter = new Counter(threshold: 3);

        counter.Changed += PrintValue;
        counter.Changed += FaultyHandler;
        counter.Changed += (_, e) => Console.WriteLine($"  [log {DateTime.Now:HH:mm:ss.fff}] {e.OldValue} -> {e.NewValue}");
        counter.ThresholdReached += (sender, e) =>
            Console.WriteLine($"  >>> Поріг {e.Threshold} досягнуто о {e.ReachedAt:HH:mm:ss} ({sender?.GetType().Name})");

        Console.WriteLine("=== Три обробники Changed + ThresholdReached ===");
        for (var i = 0; i < 4; i++)
        {
            IncrementSafely(counter);
        }

        Console.WriteLine();
        Console.WriteLine("=== Після відписки FaultyHandler (-=) ===");
        counter.Changed -= FaultyHandler;
        IncrementSafely(counter);
        IncrementSafely(counter);
    }

    private static void IncrementSafely(Counter counter)
    {
        Console.WriteLine($"Increment() #{counter.Value + 1}");
        try
        {
            counter.Increment();
        }
        catch (AggregateException ex)
        {
            Console.WriteLine($"  ! Обробників з помилкою: {ex.InnerExceptions.Count}, інші обробники відпрацювали");
            foreach (var inner in ex.InnerExceptions)
            {
                Console.WriteLine($"    {inner.GetType().Name}: {inner.Message}");
            }
        }
    }

    private static void PrintValue(object? sender, CounterChangedEventArgs e)
    {
        Console.WriteLine($"  Значення лічильника: {e.NewValue}");
    }

    private static void FaultyHandler(object? sender, CounterChangedEventArgs e)
    {
        if (e.NewValue % 2 == 0)
        {
            throw new InvalidOperationException($"{nameof(FaultyHandler)} навмисно впав на значенні {e.NewValue}.");
        }

        Console.WriteLine($"  {nameof(FaultyHandler)}: значення {e.NewValue} непарне, все гаразд");
    }
}
