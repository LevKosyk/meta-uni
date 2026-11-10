# Завдання 1. Конфіг-парсер

`ParseSetting` розбирає рядок `key=value`, `ParseFile` читає файл у `Dictionary<string, string>` і пропускає порожні рядки та коментарі `#`.

```bash
dotnet run --project Task1
```

Увага: помилка рядка файлу обгортається в `ConfigFormatException` з номером рядка, а першопричина зберігається в `InnerException`.
