# Завдання 2. checked / unchecked

`AddChecked` кидає `OverflowException`, `AddWrapped` «загортає» значення, `SumAll` працює в режимі `--safe` (checked) або `--fast` (unchecked).

```bash
dotnet run --project Task2 -- --safe
dotnet run --project Task2 -- --fast 2147483647 1
dotnet test Task2
```

## Debug vs Release

`CheckForOverflowUnderflow` увімкнено лише в Debug. Тому арифметика без явного `checked`/`unchecked` (`AddDefault`) у Debug кидає `OverflowException`, а в Release мовчки переповнюється. Явні `checked`/`unchecked` поводяться однаково в обох конфігураціях.
