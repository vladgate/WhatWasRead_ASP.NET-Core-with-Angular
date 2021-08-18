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
    const newAuthor = new Author(this.newFirstName, this.newLastName);
    const errors = newAuthor.validate();
    if (errors) {
      this.repo.authorSaveErrors = errors;
      return;
    }
    this.repo.saveNewAuthor(newAuthor);
    this.newFirstName = "";
    this.newLastName = "";
  }

  editAuthor(authorId: number) {
    this.editedAuthor = this.repo.authors.find(a => a.authorId === authorId);
  }

  saveEditedAuthor() {
    const errors = this.editedAuthor.validate();
    if (errors) {
      this.repo.authorSaveErrors = errors;
      return;
    }
    this.repo.updateAuthor(this.editedAuthor);
    this.editedAuthor = null;
  }

  deleteAuthor(authorId: number) {
    this.repo.deleteAuthor(authorId);
  }

}
