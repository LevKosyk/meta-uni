# Завдання 4. Профілі користувачів у JSON

`save` зберігає один профіль або список (`--list`), `load` читає файл з одним профілем або зі списком.

```bash
dotnet run --project Task4 -- save
dotnet run --project Task4 -- load
dotnet run --project Task4 -- save --list
dotnet run --project Task4 -- load --list
dotnet run --project Task4 -- load Task4/samples/profile-v1.json
```

Увага: `profile-v1.json` не має поля `phone` — при читанні підставляється значення за замовчуванням. `IsInternal` не серіалізується, час зберігається в UTC.
