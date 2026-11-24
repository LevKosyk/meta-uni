export type Product = { title: string; price: number };

export function parseProduct(line: string): Product {
  const parts = line.split(" - ");
  if (parts.length !== 2) {
    throw new Error(`Неправильний рядок "${line}"`);
  }

  const price = Number(parts[1].trim());
  if (Number.isNaN(price)) {
    throw new Error(`Неправильна ціна в рядку "${line}"`);
  }

  return { title: parts[0].trim(), price };
}

export function calcTotal(products: Array<Product>): number {
  let total = 0;
  for (const p of products) {
    total += p.price;
  }
  return total;
}
