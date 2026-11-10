# Завдання 2. JSON-контракт атрибутами

`OrderDto` серіалізує `Id` як `order_id`, статус як `new|paid|shipped`, пропускає `Comment = null` і збирає невідомі поля в `Extra`.

```bash
dotnet run --project Task2
```

Увага: стандартний `JsonStringEnumConverter` ігнорує `[EnumMember]` і пише `"Shipped"`, тому статус серіалізується через власний `EnumMemberConverter`, який читає значення з `[EnumMember(Value = "...")]`.
