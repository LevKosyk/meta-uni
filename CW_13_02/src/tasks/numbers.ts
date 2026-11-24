export function sumPositive(nums: number[]): number {
  let sum = 0;
  for (const n of nums) {
    if (n > 0) {
      sum += n;
    }
  }
  return sum;
}

export function filterEven(nums: number[]): number[] {
  return nums.filter(n => n % 2 === 0);
}
