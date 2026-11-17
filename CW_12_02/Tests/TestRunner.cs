using System.Reflection;

namespace StudentMetaFramework.Tests;

public record TestResult(string Name, bool Passed, string? Error);

public static class TestRunner
{
    public static List<TestResult> RunAll(Assembly assembly)
    {
        var results = new List<TestResult>();

        var testClasses = assembly.GetTypes()
            .Where(t => t.IsClass && !t.IsAbstract && t.GetCustomAttribute<TestClassAttribute>() is not null);

        foreach (var type in testClasses)
        {
            var methods = type.GetMethods(BindingFlags.Public | BindingFlags.Instance)
                .Where(m => m.GetCustomAttribute<TestMethodAttribute>() is not null && m.GetParameters().Length == 0);

            foreach (var method in methods)
            {
                var name = $"{type.Name}.{method.Name}";
                try
                {
                    var instance = Activator.CreateInstance(type);
                    method.Invoke(instance, null);
                    results.Add(new TestResult(name, true, null));
                }
                catch (TargetInvocationException ex)
                {
                    results.Add(new TestResult(name, false, ex.InnerException?.Message ?? ex.Message));
                }
            }
        }

        return results;
    }
}
