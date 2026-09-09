using System;

class Program
{
    static void Main(string[] args)
    {
        Console.Write("Введіть перше число: ");
        double number1 = Convert.ToDouble(Console.ReadLine());

        Console.Write("Введіть друге число: ");
        double number2 = Convert.ToDouble(Console.ReadLine());

        Console.Write("Оберіть дію (+, -, *, /): ");
        string operation = Console.ReadLine();

        double result;

        switch (operation)
        {
            case "+":
                result = number1 + number2;
                Console.WriteLine($"Результат: {result}");
                break;

            case "-":
                result = number1 - number2;
                Console.WriteLine($"Результат: {result}");
                break;

            case "*":
                result = number1 * number2;
                Console.WriteLine($"Результат: {result}");
                break;

            case "/":
                if (number2 == 0)
                {
                    Console.WriteLine("Помилка, ділення на нуль неможливе.");
                }
                else
                {
                    result = number1 / number2;
                    Console.WriteLine($"Результат: {result}");
                }
                break;

            default:
                Console.WriteLine("Невідома операція.");
                break;
        }
    }
}