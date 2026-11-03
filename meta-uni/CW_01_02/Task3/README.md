# Завдання 3. Модель EF Core атрибутами

`User.cs` — сутність з `[Key]`, `[Required]`, `[MaxLength]`, `[Index(IsUnique = true)]`, `[ConcurrencyCheck]` і `[Precision(0)]`. Очікувана схема БД та пояснення `[ConcurrencyCheck]` — у коментарі над класом.

```bash
dotnet run --project Task3
```

Увага: міграції не запускаються, програма лише виводить атрибути моделі.
