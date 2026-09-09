using System;
using System.Collections.Generic;
using System.Linq;

class Book
{
    public string Title { get; set; }
    public string Author { get; set; }
    public int Year { get; set; }

    public Book(string title, string author, int year)
    {
        Title = title;
        Author = author;
        Year = year;
    }

    public string GetDescription()
    {
        return $"Назва: {Title}, Автор: {Author}, Рік: {Year}";
    }
}

class Program
{
    static void Main()
    {
        List<Book> books = new List<Book>
        {
            new Book("1984", "Orwell", 1949),
            new Book("The Hobbit", "Tolkien", 1937),
            new Book("Harry Potter", "Rowling", 1997),
            new Book("The Witcher", "Andrzej Sapkowski", 1990)
        };

        Console.WriteLine("Бібліотека книг");
        Console.WriteLine("1 - Показати всі книги");
        Console.WriteLine("2 - Пошук за назвою");
        Console.WriteLine("3 - Пошук за автором");
        Console.WriteLine("4 - Пошук за роком");
        Console.WriteLine("0 - Вийти");
        

        List<Book> result = new List<Book>();
        string choice; 
        do
        {
            Console.Write("Оберіть дію: ");
            choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    result = books;
                    break;

                case "2":
                    Console.Write("Введіть назву книги: ");
                    string title = Console.ReadLine();

                    result = books
                        .Where(book => book.Title == title)
                        .ToList();
                    break;

                case "3":
                    Console.Write("Введіть автора: ");
                    string author = Console.ReadLine();

                    result = books
                        .Where(book => book.Author == author)
                        .ToList();
                    break;

                case "4":
                    Console.Write("Введіть рік: ");
                    int year = Convert.ToInt32(Console.ReadLine());

                    result = books
                        .Where(book => book.Year == year)
                        .ToList();
                    break;
            }
            if (result.Count == 0)
            {
                Console.WriteLine("Книг не знайдено.");
            }
            else
            {
                foreach (Book book in result)
                {
                    Console.WriteLine(book.GetDescription());
                }
            }
        } while (choice != "0");
    }
}