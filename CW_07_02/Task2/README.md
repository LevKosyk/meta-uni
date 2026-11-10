# Завдання 2. Надійне оновлення конфігурації

Збирає конфігурацію (типові ключі або `--template`, плюс `KEY=VALUE` з командного рядка) і атомарно оновлює `appsettings.txt`.

```bash
dotnet run --project Task2
dotnet run --project Task2 -- LogLevel=Debug
dotnet run --project Task2 -- --template Task2/samples/template.txt
```

Увага: запис іде в `appsettings.txt.tmp` з `FileShare.None` і `Flush`, потім `File.Replace` з резервною копією `.bak`, тому файл завжди або повністю старий, або повністю новий.
