using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Task3.EfModel;

/// <summary>
/// Очікувана схема БД після міграції EF Core (SQL Server):
///
/// Таблиця Users
///   Id          INT            NOT NULL  PRIMARY KEY, IDENTITY(1,1)
///   Email       NVARCHAR(200)  NOT NULL
///   DisplayName NVARCHAR(80)   NULL
///   Version     INT            NOT NULL
///   CreatedAt   DATETIME2(0)   NOT NULL
///
/// Індекси
///   PK_Users          — кластерний первинний ключ на Id
///   IX_Users_Email    — UNIQUE індекс на Email
///
/// Конкурентність
///   [ConcurrencyCheck] на Version: EF Core додає старе значення Version у WHERE
///   кожного UPDATE/DELETE: UPDATE Users SET ... WHERE Id = @id AND Version = @oldVersion.
///   Якщо інший процес уже змінив рядок, оновлюється 0 рядків і EF Core кидає
///   DbUpdateConcurrencyException. Значення Version сам EF не збільшує —
///   застосунок має робити Version++ перед SaveChanges.
///
/// Точність часу
///   [Precision(0)] на CreatedAt дає DATETIME2(0): дробова частина секунд не зберігається,
///   у БД потрапляє час з точністю до цілих секунд.
/// </summary>
[Index(nameof(Email), IsUnique = true)]
public class User
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(200)]
    public string Email { get; set; } = string.Empty;

    [MaxLength(80)]
    public string? DisplayName { get; set; }

    [ConcurrencyCheck]
    public int Version { get; set; }

    [Precision(0)]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
