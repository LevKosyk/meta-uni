namespace Task4.PasswordTests;

[Trait("category", "validation")]
public class PasswordRulesTests
{
    [Theory]
    [InlineData("Abcdef12", true)]
    [InlineData("pass1234word", true)]
    [InlineData("Abcdefgh", false)]
    [InlineData("12345678", false)]
    public void IsStrongPassword_BasicCases(string input, bool expected)
    {
        Assert.Equal(expected, PasswordRules.IsStrongPassword(input));
    }

    [Theory]
    [InlineData("A1b2C3d4")]
    [InlineData("1234567a")]
    [InlineData("Пароль12")]
    public void IsStrongPassword_ValidAtBoundary_ReturnsTrue(string input)
    {
        Assert.True(PasswordRules.IsStrongPassword(input));
    }

    [Theory]
    [InlineData("A1b2C3d")]
    [InlineData("ABCDEFGH")]
    [InlineData("12345678")]
    [InlineData("!!!!!!!!")]
    [InlineData("")]
    [InlineData(null)]
    public void IsStrongPassword_InvalidAtBoundary_ReturnsFalse(string? input)
    {
        Assert.False(PasswordRules.IsStrongPassword(input));
    }
}
