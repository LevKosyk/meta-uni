using StudentMetaFramework.Core.Mapping;
using StudentMetaFramework.Core.Validation;

namespace StudentMetaFramework.Core.Models;

public class User
{
    [CsvColumn("login")]
    [Required]
    [StringLength(20, MinLength = 3)]
    public string Login { get; set; } = string.Empty;

    [CsvColumn("email")]
    [Required]
    [StringLength(100)]
    public string Email { get; set; } = string.Empty;

    [CsvColumn("age")]
    [Range(0, 120)]
    public int Age { get; set; }

    [CsvIgnore]
    public bool IsAdult => Age >= 18;

    public override string ToString() => $"{Login}, {Email}, {Age}";
}
