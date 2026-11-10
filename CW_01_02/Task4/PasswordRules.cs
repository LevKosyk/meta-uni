namespace Task4.PasswordTests;

public static class PasswordRules
{
    public const int MinLength = 8;

    public static bool IsStrongPassword(string? s)
    {
        if (string.IsNullOrEmpty(s) || s.Length < MinLength)
        {
            return false;
        }

        return s.Any(char.IsLetter) && s.Any(char.IsDigit);
    }
}
