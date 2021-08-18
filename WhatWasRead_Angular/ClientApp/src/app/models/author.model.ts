export class Author {
  constructor(public firstName: string, public lastName: string, public link?: string, public checked?: boolean, public authorId?: number) { }

  validate(): string {
    let errors = "";
    if (this.lastName.trim().length < 2 || this.lastName.trim().length > 30) {
      errors += "Фамилия автора должна состоять от 2 до 30 символов. "
    }
    if (this.firstName.trim().length < 2 || this.firstName.trim().length > 30) {
      errors += "Имя автора должно состоять от 2 до 30 символов."
    }
    return errors;
  }
}
