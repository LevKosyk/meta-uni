# Завдання 1. DTO + DataAnnotations

`RegisterDto` описує форму реєстрації атрибутами `[Required]`, `[EmailAddress]`, `[StringLength]`, `[Compare]`, `[Range]`, а `ValidationHelper.Validate` повертає помилки через `Validator.TryValidateObject`.

```bash
dotnet run --project Task1
```

Увага: кожне повідомлення має вигляд `[Поле]: причина`, тексти задані через `ErrorMessage`.
