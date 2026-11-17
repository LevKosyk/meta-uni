using StudentMetaFramework.Core.Models;
using StudentMetaFramework.Core.Validation;

namespace StudentMetaFramework.Tests;

[TestClass]
public class ValidatorTests
{
    [TestMethod]
    public void ValidUser_HasNoErrors()
    {
        var errors = Validator.Validate(new User { Login = "anna", Email = "anna@mail.com", Age = 25 });

        if (errors.Count != 0)
        {
            throw new Exception($"Очікувалось 0 помилок, а є {errors.Count}");
        }
    }

    [TestMethod]
    public void EmptyLogin_IsRequired()
    {
        var errors = Validator.Validate(new User { Login = "", Email = "a@mail.com", Age = 20 });

        if (!errors.Any(e => e.Contains("Login")))
        {
            throw new Exception("Немає помилки для Login");
        }
    }

    [TestMethod]
    public void AgeOutOfRange_HasError()
    {
        var errors = Validator.Validate(new User { Login = "max", Email = "max@mail.com", Age = 150 });

        if (errors.Count != 1)
        {
            throw new Exception($"Очікувалась 1 помилка, а є {errors.Count}");
        }
    }

    [TestMethod]
    public void NegativePrice_HasError()
    {
        var errors = Validator.Validate(new Product { Name = "Мишка", Price = -10, Quantity = 1, Category = "Електроніка" });

        if (!errors.Any(e => e.Contains("Price")))
        {
            throw new Exception("Немає помилки для Price");
        }
    }
}
