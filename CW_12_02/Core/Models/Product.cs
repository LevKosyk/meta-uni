using StudentMetaFramework.Core.Mapping;
using StudentMetaFramework.Core.Validation;

namespace StudentMetaFramework.Core.Models;

public class Product
{
    [CsvColumn("name")]
    [Required]
    [StringLength(50, MinLength = 2)]
    public string Name { get; set; } = string.Empty;

    [CsvColumn("price")]
    [Range(0.01, 1_000_000)]
    public decimal Price { get; set; }

    [CsvColumn("quantity")]
    [Range(0, 10_000)]
    public int Quantity { get; set; }

    [CsvColumn("category")]
    [Required]
    public string Category { get; set; } = string.Empty;

    [CsvIgnore]
    public decimal Total => Price * Quantity;

    public override string ToString() => $"{Name}, {Price}, {Quantity} шт, {Category}";
}
