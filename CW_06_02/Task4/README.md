# Завдання 4. Делегати як стратегії

`Transform` застосовує стратегію `TrimToUpper` або `MaskDigits`, `TextFormatter` застосовує поточну стратегію до списку рядків.

```bash
dotnet run --project Task4
```

Увага: стратегія змінюється присвоєнням `formatter.Strategy`, без if/else.
