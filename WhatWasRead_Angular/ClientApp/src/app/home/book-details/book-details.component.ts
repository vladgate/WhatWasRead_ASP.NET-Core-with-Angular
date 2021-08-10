import { ChangeDetectorRef, Component, OnInit } from "@angular/core";
import { ActivatedRoute } from "@angular/router";
import { Repository } from "../../models/repository";

@Component({
  //template:'<div>book details</div>',
  templateUrl: './book-details.component.html',
  styleUrls: ['./book-details.component.css']
})
export class BookDetailsComponent implements OnInit {

  isWantDelete: boolean = false;

  constructor(private repo: Repository, private activeRoute: ActivatedRoute) {
  }

  ngOnInit(): void {
    this.repo.getBookDetails(this.activeRoute.snapshot.params['id']);
  }

  get authors() {
    return this.repo.currentBookDetails.authorsOfBooks.indexOf(',') >= 0 ? "Авторы:" : "Автор:";
  }
}
