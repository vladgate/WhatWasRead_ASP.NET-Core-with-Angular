import { Component, Input } from "@angular/core";
import { BookShortInfo } from "../../../models/bookShortInfo.model";

@Component({
  selector: "book-info",
  templateUrl: "./book-info.component.html",
  styleUrls: ["./book-info.component.css"]
})
export class BookInfoComponent {
  @Input() bookInput: BookShortInfo;

  private imgBasePath = '/api/books/getImage/';

  constructor() { }

  get name(): string {
    return this.bookInput.name;
  }

  get imgSource(): string {
    return this.imgBasePath + this.bookInput.bookId;
  }

  get authors(): string {
    const start = this.bookInput.authors.indexOf(",") != -1 ? "Авторы: " : "Автор: ";
    return start + this.bookInput.authors;
  }
}
