# Завдання 5. Події

`Counter` піднімає подію `Changed` при кожному `Increment()` і `ThresholdReached` при досягненні порогу.

```bash
dotnet run --project Task5
```

Увага: кожен обробник викликається у своєму `try/catch`, тому виняток одного не заважає іншим, а помилки повертаються як `AggregateException`.
