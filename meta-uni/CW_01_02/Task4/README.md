# Завдання 4. Параметризовані тести (xUnit)

`PasswordRules.IsStrongPassword` перевіряє: мінімум 8 символів, хоча б одна літера й одна цифра. Тести — `[Theory]` + `[InlineData]`, згруповані `[Trait("category", "validation")]`.

```bash
dotnet test Task4
dotnet test Task4 --filter "category=validation"
```

Лог запуску:

```
Passed IsStrongPassword_BasicCases(input: "Abcdef12", expected: True)
Passed IsStrongPassword_BasicCases(input: "pass1234word", expected: True)
Passed IsStrongPassword_BasicCases(input: "Abcdefgh", expected: False)
Passed IsStrongPassword_BasicCases(input: "12345678", expected: False)
Passed IsStrongPassword_ValidAtBoundary_ReturnsTrue(input: "A1b2C3d4")
Passed IsStrongPassword_ValidAtBoundary_ReturnsTrue(input: "1234567a")
Passed IsStrongPassword_ValidAtBoundary_ReturnsTrue(input: "Пароль12")
Passed IsStrongPassword_InvalidAtBoundary_ReturnsFalse(input: "A1b2C3d")
Passed IsStrongPassword_InvalidAtBoundary_ReturnsFalse(input: "ABCDEFGH")
Passed IsStrongPassword_InvalidAtBoundary_ReturnsFalse(input: "12345678")
Passed IsStrongPassword_InvalidAtBoundary_ReturnsFalse(input: "!!!!!!!!")
Passed IsStrongPassword_InvalidAtBoundary_ReturnsFalse(input: "")
Passed IsStrongPassword_InvalidAtBoundary_ReturnsFalse(input: null)
Total tests: 13, Passed: 13
```
