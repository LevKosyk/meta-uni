using System;
using System.Collections.Generic;
using System.Linq;

class Program
{
    static void Main()
    {
        Console.WriteLine("Введіть текст.\nДля завершення введіть Enter на пустому рядку:");

        List<string> lines = new List<string>();

        while (true)
        {
            string line = Console.ReadLine();

            if (string.IsNullOrEmpty(line))
                break;

            lines.Add(line);
        }

        int words = 0;
        int spaces = 0;
        int allCharacters = 0;
        int visibleCharacters = 0;

        foreach (string line in lines)
        {
            bool isWord = false;

            foreach (char symbol in line)
            {
                allCharacters++;

                if (symbol == ' ')
                {
                    spaces++;
                    isWord = false;
                }
                else
                {
                    visibleCharacters++;

                    if (!isWord)
                    {
                        words++;
                        isWord = true;
                    }
                }
            }
        }

        int lineCount = lines.Count;

        Console.WriteLine($"Кількість слів: {words}");
        Console.WriteLine($"Кількість пробілів: {spaces}");
        Console.WriteLine($"Кількість символів: {allCharacters}");
        Console.WriteLine($"Кількість видимих символів: {visibleCharacters}");
        Console.WriteLine($"Кількість рядків: {lineCount}");
    }
}