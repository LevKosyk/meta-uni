using System;

class Program
{
    static void Main()
    {
        const double USD_TO_UAH = 41.5;
        const double EUR_TO_UAH = 48.5;
        Console.Write("Введіть суму: ");
        double amount = Convert.ToDouble(Console.ReadLine());

        Console.Write("Введіть валюту (USD, EUR, UAH): ");
        string fromCurrency = Console.ReadLine().ToUpper();

        Console.Write("У яку валюту конвертувати (USD, EUR, UAH): ");
        string toCurrency = Console.ReadLine().ToUpper();

        double amountInUah;

        switch (fromCurrency)
        {
            case "USD":
                amountInUah = amount * USD_TO_UAH;
                break;

            case "EUR":
                amountInUah = amount * EUR_TO_UAH;
                break;

            case "UAH":
                amountInUah = amount;
                break;

            default:
                Console.WriteLine("Невідома валюта.");
                return;
        }

        double result;

        switch (toCurrency)
        {
            case "USD":
                result = amountInUah / USD_TO_UAH;
                break;

            case "EUR":
                result = amountInUah / EUR_TO_UAH;
                break;

            case "UAH":
                result = amountInUah;
                break;

            default:
                Console.WriteLine("Невідома валюта.");
                return;
        }

        Console.WriteLine($"Результат: {result:F2} {toCurrency}");
    }
}