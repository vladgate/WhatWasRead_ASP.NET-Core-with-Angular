import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { BookCreateOrEdit } from '../../models/bookCreateOrEdit.model';
import { Category } from '../../models/category.model';
import { Language } from '../../models/language.model';
import { AuthorShortInfo, Repository } from '../../models/repository';
import { Tag } from '../../models/tag.model';

@Component({
  selector: 'app-book-createOrEdit',
  templateUrl: './book-createOrEdit.component.html',
  styleUrls: ['./book-createOrEdit.component.css']
})
export class BookCreateOrEditComponent implements OnInit {
  @ViewChild('nameInput', { static: false }) nameInput: ElementRef;
  @ViewChild('pagesInput', { static: false }) pagesInput: ElementRef;
  @ViewChild('yearInput', { static: false }) yearInput: ElementRef;
  @ViewChild('descriptionInput', { static: false }) descriptionInput: ElementRef;
  @ViewChild('authorsInput', { static: false }) authorsInput: ElementRef;
  @ViewChild('langInput', { static: false }) langInput: ElementRef;
  @ViewChild('categoryInput', { static: false }) categoryInput: ElementRef;
  @ViewChild('tagsInput', { static: false }) tagsInput: ElementRef;

  errors: string;
  isCreate: boolean;

  constructor(private repo: Repository, private activeRoute: ActivatedRoute) {
    if (this.activeRoute.snapshot.routeConfig.path === 'books/create') {
      this.isCreate = true;
    }
    else {
      this.isCreate = false;
    }
  }

  ngOnInit() {
    if (this.isCreate) {
      this.repo.getMetadataForBook();
      this.repo.editedBook = new BookCreateOrEdit();
    }
    else {
      const bookId = this.activeRoute.snapshot.params['id'];
      if (bookId) {
        this.repo.getBookForEditing(bookId);
      }
      else {
        this.errors = "Неверный id."
      }
    }
  }
  isSelectedbook(authorId): boolean {
    return this.repo.editedBook.selectedAuthorsId && this.repo.editedBook.selectedAuthorsId.includes(authorId);
  }

  get book(): BookCreateOrEdit {
    return this.repo.editedBook;
  }

  get categories(): Category[] {
    return this.repo.bookMetadata.categories;
  }

  get authors(): AuthorShortInfo[] {
    return this.repo.bookMetadata.authors;
  }

  get languages(): Language[] {
    return this.repo.bookMetadata.languages;
  }

  get tags(): Tag[] {
    return this.repo.bookMetadata.tags;
  }

  fileChanged($event) {
    if ($event.target.files && $event.target.files[0]) {
      let reader = new FileReader();
      reader.readAsDataURL($event.target.files[0]);
      reader.onload = () => {
        this.repo.editedBook.base64ImageSrc = <string>reader.result;
      };
    }
  }

  saveBook() {
    this.book.name = this.nameInput.nativeElement.value;
    this.book.pages = parseInt(this.pagesInput.nativeElement.value);
    this.book.year = parseInt(this.yearInput.nativeElement.value);
    this.book.description = this.descriptionInput.nativeElement.value;

    const selLanguageOption = Array.prototype.find.call(this.langInput.nativeElement.options, (el) => el.selected);
    if (selLanguageOption) {
      this.book.selectedLanguageId = parseInt(selLanguageOption.value);
    }
    const selCategoryOption = Array.prototype.find.call(this.categoryInput.nativeElement.options, (el) => el.selected);
    if (selCategoryOption) {
      this.book.selectedCategoryId = parseInt(selCategoryOption.value);
    }
    this.book.selectedAuthorsId = Array.prototype.filter.call(this.authorsInput.nativeElement.options, (el) => el.selected).map(option => parseInt(option.attributes["ng-reflect-value"].value));
    this.book.selectedTagsId = Array.prototype.filter.call(this.tagsInput.nativeElement.options, (el) => el.selected).map(option => parseInt(option.attributes["ng-reflect-value"].value));

    const errors = this.book.validate();
    if (errors) {
      this.errors = errors;
      return;
    }
    try {
      this.repo.saveBook(this.book);
    }
    catch (e) {
      this.errors = e.message;
    }
  }
}
