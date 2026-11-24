import { celsiusToFahrenheit, fahrenheitToCelsius } from "./tasks/temperature";
import { isStrongPassword } from "./tasks/password";
import { sumPositive, filterEven } from "./tasks/numbers";
import { addContact, findContactByName, listContacts } from "./tasks/contacts";
import { normalizeSentence, countWords } from "./tasks/text";
import { parseProduct, calcTotal, Product } from "./tasks/products";
import { Student, getMinGrade, getMaxGrade, getAverageGrade } from "./tasks/grades";

console.log("Завдання 1");
for (const c of [0, 20, 37, 100]) {
  console.log(`${c} C = ${celsiusToFahrenheit(c)} F`);
}
for (const f of [32, 68, 212]) {
  console.log(`${f} F = ${fahrenheitToCelsius(f)} C`);
}

console.log("\nЗавдання 2");
for (const pwd of ["Password1", "password1", "PASSWORD", "Pass1", "SuperSecret2024"]) {
  console.log(`${pwd} ${isStrongPassword(pwd)}`);
}

console.log("\nЗавдання 3");
const nums = [3, -1, 4, 0, 10, -5];
console.log(`Масив [${nums.join(", ")}]`);
console.log(`Сума додатних ${sumPositive(nums)}`);
console.log(`Парні [${filterEven(nums).join(", ")}]`);

console.log("\nЗавдання 4");
addContact("Олена", "+380671112233");
addContact("Іван", "+380502223344");
addContact("Марія", "+380933334455");
const found = findContactByName("Іван");
console.log(found ? `Знайшов ${found.name} ${found.phone}` : "Не знайшов");
console.log(findContactByName("Петро") ?? "Петра немає");
listContacts();

console.log("\nЗавдання 5");
for (const text of ["  Hello   world  from   TypeScript  ", "   ONE    two   ", "    "]) {
  console.log(`"${normalizeSentence(text)}" слів ${countWords(text)}`);
}

console.log("\nЗавдання 6");
const lines = ["Milk - 42.5", "Bread - 25", "Cheese - 120.75", "Eggs - 58"];
const products: Product[] = [];
for (const line of lines) {
  try {
    products.push(parseProduct(line));
  } catch (e) {
    console.log((e as Error).message);
  }
}
for (const p of products) {
  console.log(`${p.title} ${p.price}`);
}
console.log(`Разом ${calcTotal(products)}`);

console.log("\nЗавдання 7");
const students: Student[] = [
  { name: "Олена", grades: [10, 12, 9, 11] },
  { name: "Іван", grades: [7, 8, 10, 6, 9] },
];
for (const s of students) {
  console.log(`${s.name} [${s.grades.join(", ")}] мін ${getMinGrade(s.grades)}, макс ${getMaxGrade(s.grades)}, середнє ${getAverageGrade(s.grades).toFixed(2)}`);
}
