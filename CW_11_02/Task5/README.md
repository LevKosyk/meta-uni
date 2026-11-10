# Завдання 5

Програма знаходить усі класи, які реалізують IPlugin, створює їх і викликає Execute.

```bash
dotnet run --project Task5
```

Список плагінів ніде не прописаний, вони знаходяться під час виконання через Assembly.GetExecutingAssembly().GetTypes().
