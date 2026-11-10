# Завдання 3. Двійкове сховище товарів

`write` створює файл із 3 товарами (`--v1` або `--v2`, за замовчуванням v2), `read` читає обидві версії й виводить `#Id Name Price Category`.

```bash
dotnet run --project Task3 -- write --v1
dotnet run --project Task3 -- read
dotnet run --project Task3 -- write --v2
dotnet run --project Task3 -- read
```

Увага: формат — `MAGC` + `Int32 Version`, далі для кожного товару `Int32 Id, Double Price, String Name` (+ `String Category` у v2). Для v1 виводиться `Category=(none)`.
