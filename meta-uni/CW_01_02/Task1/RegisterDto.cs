using System.ComponentModel.DataAnnotations;

namespace Task1.RegistrationValidation;

public class RegisterDto
{
    [Required(ErrorMessage = "Поле Email є обов'язковим.")]
    [EmailAddress(ErrorMessage = "Email має невірний формат.")]
    public string? Email { get; set; }

    [Required(ErrorMessage = "Поле Password є обов'язковим.")]
    [StringLength(100, MinimumLength = 8, ErrorMessage = "Довжина пароля має бути від 8 до 100 символів.")]
    public string? Password { get; set; }

    [Required(ErrorMessage = "Поле ConfirmPassword є обов'язковим.")]
    [Compare(nameof(Password), ErrorMessage = "Паролі не збігаються.")]
    public string? ConfirmPassword { get; set; }

    [StringLength(50, ErrorMessage = "DisplayName не повинен перевищувати 50 символів.")]
    public string? DisplayName { get; set; }

    [Range(16, 120, ErrorMessage = "Вік має бути в діапазоні 16–120 років.")]
    public int? Age { get; set; }
}
