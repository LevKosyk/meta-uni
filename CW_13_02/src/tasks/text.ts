export function normalizeSentence(text: string): string {
  return text.trim().toLowerCase().replace(/\s+/g, " ");
}

export function countWords(text: string): number {
  const normalized = normalizeSentence(text);
  if (normalized === "") {
    return 0;
  }
  return normalized.split(" ").length;
}
