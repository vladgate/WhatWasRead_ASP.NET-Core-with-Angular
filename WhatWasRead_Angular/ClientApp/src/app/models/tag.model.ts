export class Tag {
  constructor(public nameForLinks: string, public nameForLabels: string, public tagId?: number) { }

  validate(): string {
    let errors = "";
    if (this.nameForLabels.trim().length < 1 || this.nameForLabels.trim().length > 50) {
      errors += "Текст представления тега должно состоять от 1 до 50 символов. "
    }
    if (this.nameForLinks.trim().length < 1 || this.nameForLinks.trim().length > 50) {
      errors += "Текст ссылки тега должен состоять от 1 до 50 символов."
    }
    return errors;
  }
}
