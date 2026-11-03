using System.Text;

namespace Task3.BinaryStore;

public record Product(int Id, double Price, string Name, string? Category);

public static class ProductStore
{
    public const int V1 = 1;
    public const int V2 = 2;

    private static readonly byte[] Magic = "MAGC"u8.ToArray();
    private static readonly UTF8Encoding Utf8NoBom = new(encoderShouldEmitUTF8Identifier: false);

    public static void Write(string path, IEnumerable<Product> products, int version)
    {
        if (version is not (V1 or V2))
        {
            throw new ArgumentOutOfRangeException(nameof(version), version, $"Підтримуються лише версії {V1} та {V2}.");
        }

        using var stream = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None);
        using var writer = new BinaryWriter(stream, Utf8NoBom);

        writer.Write(Magic);
        writer.Write(version);

        foreach (var product in products)
        {
            writer.Write(product.Id);
            writer.Write(product.Price);
            writer.Write(product.Name);

            if (version == V2)
            {
                writer.Write(product.Category ?? string.Empty);
            }
        }
    }

    public static (int Version, List<Product> Products) Read(string path)
    {
        using var stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
        using var reader = new BinaryReader(stream, Utf8NoBom);

        var magic = reader.ReadBytes(Magic.Length);
        if (!magic.AsSpan().SequenceEqual(Magic))
        {
            throw new InvalidDataException("файл не є сховищем товарів: заголовок MAGC не знайдено.");
        }

        var version = reader.ReadInt32();
        if (version is not (V1 or V2))
        {
            throw new InvalidDataException($"непідтримувана версія формату: {version}.");
        }

        var products = new List<Product>();
        while (stream.Position < stream.Length)
        {
            var id = reader.ReadInt32();
            var price = reader.ReadDouble();
            var name = reader.ReadString();
            var category = version == V2 ? reader.ReadString() : null;

            products.Add(new Product(id, price, name, category));
        }

        return (version, products);
    }
}
