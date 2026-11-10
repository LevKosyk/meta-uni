using System.Text;

namespace Task3.AttributeValidation;

// Тут свій атрибут Required і перевірка полів через рефлексію
public static class Program
{
    public static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;

        Check("Все заповнено", new User { Name = "Іван", Email = "ivan@example.com", Password = "secret123" });
        Check("Без Email і пароля", new User { Name = "Іван", Password = "  " });
        Check("Пустий User", new User());
    }

    private static void Check(string title, User user)
    {
        Console.WriteLine(title);
        var errors = Validator.Validate(user);

        if (errors.Count == 0)
        {
            Console.WriteLine("Все ок");
        }

        foreach (var error in errors)
        {
            Console.WriteLine(error);
        }

        Console.WriteLine();
    }
}
