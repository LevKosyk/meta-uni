namespace Task5.Events;

public class Counter
{
    public Counter(int threshold)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(threshold);

        Threshold = threshold;
    }

    public event EventHandler<CounterChangedEventArgs>? Changed;

    public event EventHandler<ThresholdReachedEventArgs>? ThresholdReached;

    public int Value { get; private set; }

    public int Threshold { get; }

    public void Increment()
    {
        var oldValue = Value;
        Value = checked(Value + 1);

        var errors = new List<Exception>();

        RaiseSafely(Changed, new CounterChangedEventArgs(oldValue, Value), errors);

        if (Value == Threshold)
        {
            RaiseSafely(ThresholdReached, new ThresholdReachedEventArgs(Threshold, DateTime.Now), errors);
        }

        if (errors.Count > 0)
        {
            throw new AggregateException(
                $"Обробників, що завершилися з помилкою: {errors.Count}. Решта обробників відпрацювали.",
                errors);
        }
    }

    private void RaiseSafely<TEventArgs>(EventHandler<TEventArgs>? handler, TEventArgs args, List<Exception> errors)
    {
        if (handler is null)
        {
            return;
        }

        foreach (var subscriber in handler.GetInvocationList().Cast<EventHandler<TEventArgs>>())
        {
            try
            {
                subscriber(this, args);
            }
            catch (Exception ex)
            {
                errors.Add(ex);
            }
        }
    }
}
