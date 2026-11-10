namespace Task5.Plugins;

public interface IPlugin
{
    void Execute();
}

public class HelloPlugin : IPlugin
{
    public void Execute() => Console.WriteLine("Привіт");
}

public class TimePlugin : IPlugin
{
    public void Execute() => Console.WriteLine($"Зараз {DateTime.UtcNow:HH:mm:ss}");
}

public class RandomPlugin : IPlugin
{
    public void Execute() => Console.WriteLine($"Число {Random.Shared.Next(1, 101)}");
}
