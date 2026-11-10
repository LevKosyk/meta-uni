using System.Text;

namespace Task1.RegistrationValidation;

public static class Program
{
    public static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;
        Console.WriteLine("=== RegisterDto + DataAnnotations ===");
        Console.WriteLine();

        PrintCaseResult("CASE 1: Валідні дані", new RegisterDto
        {
            Email = "student@example.com",
            Password = "StrongPass123",
            ConfirmPassword = "StrongPass123",
            DisplayName = "New Student",
            Age = 20,
        });

        PrintCaseResult("CASE 2: Невалідний email + короткий пароль", new RegisterDto
        {
            Email = "not-an-email",
            Password = "123",
            ConfirmPassword = "123",
            DisplayName = "Demo",
            Age = 18,
        });

        PrintCaseResult("CASE 3: Порожні обов'язкові поля", new RegisterDto());
    }

    private static void PrintCaseResult(string title, RegisterDto dto)
    {
        Console.WriteLine(title);
        var errors = ValidationHelper.Validate(dto);

        if (errors.Count == 0)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("OK: помилок не знайдено");
            Console.ResetColor();
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Знайдено помилки ({errors.Count}):");
            Console.ResetColor();

            foreach (var error in errors)
            {
                Console.WriteLine($" - {error}");
            }
        }

        Console.WriteLine(new string('-', 60));
    }
}
