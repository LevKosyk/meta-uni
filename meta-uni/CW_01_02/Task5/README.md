# Завдання 5. MiniLog з Caller-info і [Conditional("TRACE")]

`MiniLog.TraceIfEnabled` друкує `[TRACE] {file}:{line} {member}() - {message}`, а `[CallerMemberName]`, `[CallerFilePath]`, `[CallerLineNumber]` підставляються автоматично.

```bash
dotnet run --project Task5 -c Debug
dotnet run --project Task5 -c Release
```

Вивід у Debug:

```
[TRACE] Program.cs:12 Main() - Початок виконання
[TRACE] Program.cs:21 DoWork() - Виконуємо DoWork
[TRACE] Worker.cs:7 Run() - Worker запущено
```

У Release рядків `[TRACE]` немає.

Як вимкнено TRACE: за замовчуванням .NET SDK визначає `TRACE` і в Debug, і в Release, тому в `Task5.MiniLog.csproj` для Release задано `<DefineConstants>RELEASE</DefineConstants>` без `TRACE`. Через `[Conditional("TRACE")]` компілятор повністю прибирає виклики `TraceIfEnabled`: у Release-збірці немає навіть рядків повідомлень.
