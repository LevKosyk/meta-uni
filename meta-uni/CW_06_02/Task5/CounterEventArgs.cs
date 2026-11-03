namespace Task5.Events;

public class CounterChangedEventArgs : EventArgs
{
    public CounterChangedEventArgs(int oldValue, int newValue)
    {
        OldValue = oldValue;
        NewValue = newValue;
    }

    public int OldValue { get; }

    public int NewValue { get; }
}

public class ThresholdReachedEventArgs : EventArgs
{
    public ThresholdReachedEventArgs(int threshold, DateTime reachedAt)
    {
        Threshold = threshold;
        ReachedAt = reachedAt;
    }

    public int Threshold { get; }

    public DateTime ReachedAt { get; }
}
