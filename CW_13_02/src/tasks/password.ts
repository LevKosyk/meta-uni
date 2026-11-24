export function isStrongPassword(pwd: string): boolean {
  return pwd.length >= 8 && /[0-9]/.test(pwd) && /[A-Z]/.test(pwd);
}
