export type Student = { name: string; grades: number[] };

export function getMinGrade(grades: number[]): number {
  return Math.min(...grades);
}

export function getMaxGrade(grades: number[]): number {
  return Math.max(...grades);
}

export function getAverageGrade(grades: number[]): number {
  if (grades.length === 0) {
    return 0;
  }
  let sum = 0;
  for (const g of grades) {
    sum += g;
  }
  return sum / grades.length;
}
