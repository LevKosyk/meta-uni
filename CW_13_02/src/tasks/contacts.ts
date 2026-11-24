export type Contact = {
  id: number;
  name: string;
  phone: string;
};

const contacts: Contact[] = [];
let nextId = 1;

export function addContact(name: string, phone: string): Contact {
  const contact: Contact = { id: nextId++, name, phone };
  contacts.push(contact);
  return contact;
}

export function findContactByName(name: string): Contact | null {
  const contact = contacts.find(c => c.name.toLowerCase() === name.toLowerCase());
  return contact ?? null;
}

export function listContacts(): void {
  for (const c of contacts) {
    console.log(`${c.id}. ${c.name} ${c.phone}`);
  }
}
