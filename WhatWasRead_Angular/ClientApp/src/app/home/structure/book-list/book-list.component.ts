import { Component } from "@angular/core";
import { BookRow } from "../../../models/bookRow.model";
import { Repository } from "../../../models/repository";

@Component({
  selector: "book-list",
  templateUrl: "./book-list.component.html",
  styleUrls: ["./book-list.component.css"]
})
export class BookListComponent {
  constructor(private repo: Repository) {
  }

  get bookRows(): BookRow[] {
    const bookInfo = this.repo.mainPageModel.rightPanelData.bookInfo;
    const bookRows: BookRow[] = [];
    for (let i = 0; i < bookInfo.length; i += 2) {
      bookRows.push(new BookRow(bookInfo[i], bookInfo[i + 1]));
    }
    return bookRows;
  }

}
