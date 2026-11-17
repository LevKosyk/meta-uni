# CW_12_02

Імпорт csv і валідація через атрибути і рефлексію. Рівень 1 і 2. Відповіді в Answers.md.

```bash
dotnet run --project App
```

Core це моделі, атрибути, маппер і валідатор. Tests це мій тестовий фреймворк. App запускає тести і перевіряє users.csv і products.csv.

файл csv -> CsvMapper -> модель -> Validator -> консоль

Вивід програми

```
Тести
ок MapperTests.ImportUsers_ReadsAllRows
ок MapperTests.WrongNumber_GoesToErrors
ок MapperTests.MissingColumn_Throws
ок MapperTests.ImportProducts_ReadsDecimal
ок ValidatorTests.ValidUser_HasNoErrors
ок ValidatorTests.EmptyLogin_IsRequired
ок ValidatorTests.AgeOutOfRange_HasError
ок ValidatorTests.NegativePrice_HasError
Пройшло 8, впало 0

Користувачі
Імпортовано 5
Не імпортовано, рядок 6, age = 'abc' не підходить для Int32
anna, anna@mail.com, 25 - ок
bo, bo@mail.com, 17 - є помилки
   Login має бути від 3 до 20 символів
, empty@mail.com, 30 - є помилки
   Login обов'язкове
max, max@mail.com, 150 - є помилки
   Age має бути від 0 до 120, а зараз 150
ivan, , 40 - є помилки
   Email обов'язкове
Валідних 1, з помилками 4

Товари
Імпортовано 4
Не імпортовано, рядок 5, quantity = 'many' не підходить для Int32
Ноутбук, 25000, 5 шт, Електроніка - ок
Мишка, -10, 20 шт, Електроніка - є помилки
   Price має бути від 0,01 до 1000000, а зараз -10
X, 100, 3 шт,  - є помилки
   Name має бути від 2 до 50 символів
   Category обов'язкове
Крісло, 4200,50, 2 шт, Меблі - ок
Валідних 2, з помилками 2
```

Висновки. Використав атрибути, рефлексію, generics і роботу з файлами. Найважче було перетворювати текст у типи.
