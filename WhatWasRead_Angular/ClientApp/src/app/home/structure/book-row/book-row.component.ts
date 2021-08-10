import { Component, Input, OnInit } from "@angular/core";
import { BookRow } from "../../../models/bookRow.model";

@Component({
  selector: "book-row",
  templateUrl: "./book-row.component.html",
  styleUrls: ["./book-row.component.css"]
})
export class BookRowComponent implements OnInit {
  @Input() model: BookRow;

  constructor() {
  }

  ngOnInit(): void {
  }
}
