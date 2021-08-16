import { Component, OnInit } from '@angular/core';
import { Author } from '../../models/author.model';
import { Repository } from '../../models/repository';

@Component({
  selector: 'app-author-list',
  templateUrl: './author-list.component.html',
  styleUrls: ['./author-list.component.css', '../admin.css']
})
export class AuthorListComponent implements OnInit {

  editedAuthor: Author;
  newLastName: string = "";
  newFirstName: string = "";

  constructor(private repo: Repository) { }

  ngOnInit() {
    this.repo.getAuthors();
  }

  get authors(): Author[] {
    return this.repo.authors;
  }

  saveNewAuthor() {
    this.validateAuthor(this.newLastName, this.newFirstName);
    if (this.repo.authorSaveErrors) {
      return;
    }
    this.repo.saveNewAuthor(new Author(this.newFirstName, this.newLastName));
    this.newFirstName = "";
    this.newLastName = "";
  }

  validateAuthor(lastName, firstName): void {
    this.repo.authorSaveErrors = "";
    if (lastName.trim().length < 2 || lastName.trim().length > 30) {
      this.repo.authorSaveErrors += "Фамилия автора должна состоять от 2 до 30 символов. "
    }
    if (firstName.trim().length < 2 || firstName.trim().length > 30) {
      this.repo.authorSaveErrors += "Имя автора должно состоять от 2 до 30 символов."
    }
  }

  editAuthor(authorId: number) {
    this.editedAuthor = this.repo.authors.find(a => a.authorId === authorId);
  }

  saveEditedAuthor() {
    this.validateAuthor(this.editedAuthor.lastName, this.editedAuthor.firstName);
    if (this.repo.authorSaveErrors) {
      return;
    }
    this.repo.updateAuthor(this.editedAuthor);
    this.editedAuthor = null;
  }

  deleteAuthor(authorId: number) {
    this.repo.deleteAuthor(authorId);
  }

}
