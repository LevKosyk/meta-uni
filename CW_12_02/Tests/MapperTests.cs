using StudentMetaFramework.Core.Mapping;
using StudentMetaFramework.Core.Models;

namespace StudentMetaFramework.Tests;

[TestClass]
public class MapperTests
{
    [TestMethod]
    public void ImportUsers_ReadsAllRows()
    {
        var path = WriteTemp("login,email,age", "anna,anna@mail.com,25", "ivan,ivan@mail.com,40");
        try
        {
            var result = new CsvMapper<User>().Import(path);

            if (result.Items.Count != 2 || result.Items[0].Login != "anna" || result.Items[1].Age != 40)
            {
                throw new Exception("Користувачі прочитались неправильно");
            }
        }
        finally
        {
            File.Delete(path);
        }
    }

    [TestMethod]
    public void WrongNumber_GoesToErrors()
    {
        var path = WriteTemp("login,email,age", "olga,olga@mail.com,abc");
        try
        {
            var result = new CsvMapper<User>().Import(path);

            if (result.Items.Count != 0 || result.Errors.Count != 1)
            {
                throw new Exception("Рядок з помилкою мав потрапити в Errors");
            }
        }
        finally
        {
            File.Delete(path);
        }
    }

    [TestMethod]
    public void MissingColumn_Throws()
    {
        var path = WriteTemp("login,age", "anna,25");
        try
        {
            new CsvMapper<User>().Import(path);
            throw new Exception("Мав бути виняток через відсутню колонку email");
        }
        catch (CsvMappingException)
        {
        }
        finally
        {
            File.Delete(path);
        }
    }

    [TestMethod]
    public void ImportProducts_ReadsDecimal()
    {
        var path = WriteTemp("name,price,quantity,category", "Ноутбук,25000.50,5,Електроніка");
        try
        {
            var result = new CsvMapper<Product>().Import(path);

            if (result.Items.Count != 1 || result.Items[0].Price != 25000.50m)
            {
                throw new Exception("Ціна прочиталась неправильно");
            }
        }
        finally
        {
            File.Delete(path);
        }
    }

    private static string WriteTemp(params string[] lines)
    {
        var path = Path.GetTempFileName();
        File.WriteAllLines(path, lines);
        return path;
    }
}
