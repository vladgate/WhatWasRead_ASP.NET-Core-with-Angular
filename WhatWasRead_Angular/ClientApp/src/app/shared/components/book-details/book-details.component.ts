import { AfterViewInit, Component, OnInit } from "@angular/core";
import { ActivatedRoute, Router } from "@angular/router";
import { AuthenticationService } from "../../../auth/authentication.service";
import { Repository } from "../../../models/repository";

@Component({
  templateUrl: './book-details.component.html',
  styleUrls: ['./book-details.component.css']
})
export class BookDetailsComponent implements OnInit {

  isWantDelete: boolean = false;

  constructor(private repo: Repository, private router: Router, private activeRoute: ActivatedRoute, public authService: AuthenticationService) {
  }

  ngOnInit(): void {
    this.repo.getBookDetails(this.activeRoute.snapshot.params['id']);
  }

  get isAuth() {
    return this.authService.authenticated;
  }

  editClick($event) {
    $event.preventDefault();
    this.isWantDelete = true;
  }

  onDeleteConfirmed() {
    this.repo.deleteBook(this.activeRoute.snapshot.params['id']);
    this.router.navigateByUrl('/');
    this.isWantDelete = false;
  }
}
