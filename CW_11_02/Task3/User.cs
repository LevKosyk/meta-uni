namespace Task3.AttributeValidation;

public class User
{
    [Required]
    public string? Name { get; set; }

    [Required]
    public string? Email { get; set; }

    [Required]
    public string? Password { get; set; }

    public string? Nickname { get; set; }
}
