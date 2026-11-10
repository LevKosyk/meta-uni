namespace Task1.TypeInspector;

public class Student
{
    public string Name { get; set; } = string.Empty;

    public int Age { get; set; }

    public string Group { get; set; } = string.Empty;

    public void PrintInfo()
    {
        Console.WriteLine($"{Name}, {Age}, {Group}");
    }
}
