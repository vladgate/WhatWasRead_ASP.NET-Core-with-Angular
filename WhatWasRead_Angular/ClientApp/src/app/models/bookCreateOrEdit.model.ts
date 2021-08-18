
export class BookCreateOrEdit {
  constructor(public bookId?: number, public name?: string, public description?: string, public pages?: number, public year?: number, public base64ImageSrc?: string,
     public selectedCategoryId?: number, public selectedLanguageId?: number, public selectedAuthorsId?: number[], public selectedTagsId?: number[]) { }

  public validate(): string {
    let errors: string = "";
    if (!this.name || this.name.length < 4 || this.name.length > 100) {
      errors += "Название книни должно состоять от 4 до 100 символов. ";
    }
    if (!this.pages || this.pages < 1 || this.pages > 5000) {
      errors += "Количество страниц должно быть в диапазоне от 1 до 5000. ";
    }
    if (!this.description || this.description.length < 20 || this.description.length > 1000) {
      errors += "Описание книги должно состоять от 20 до 1000 символов. ";
    }
    if (!this.year || this.year < 1980 || this.year > 2050) {
      errors += "Год должен быть в диапазоне от 1980 до 2050. ";
    }
    if (!this.base64ImageSrc) {
      errors += "Не выбрана обложка книги. ";
    }
    if (!this.selectedLanguageId) {
      errors += "Не указан язык. ";
    }
    if (!this.selectedCategoryId) {
      errors += "Не указана категория. ";
    }
    if (!this.selectedAuthorsId || this.selectedAuthorsId.length < 1) {
      errors += "Не указано авторство книги. ";
    }
    return errors;
  }
}
