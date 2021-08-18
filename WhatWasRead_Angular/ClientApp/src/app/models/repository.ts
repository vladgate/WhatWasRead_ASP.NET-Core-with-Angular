import { Injectable } from '@angular/core';
import { BookShortInfo } from './bookShortInfo.model';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Author } from './author.model';
import { Category } from './category.model';
import { Language } from './language.model';
import { Tag } from './tag.model';
import { BookDetailedInfo } from './bookDetailedInfo';
import { BookCreateOrEdit } from './bookCreateOrEdit.model';
import { Router } from '@angular/router';

export interface LeftPanelData {
  authors: Author[],
  categories: Category[],
  languages: Language[],
  tags: Tag[],
  maxPagesActual: number,
  maxPagesExpected: number,
  minPagesActual: number,
  minPagesExpected: number
}

export interface BookMetadata {
  authors: AuthorShortInfo[],
  categories: Category[],
  languages: Language[],
  tags: Tag[]
}

export type AuthorShortInfo = {
  authorId: number,
  displayText: string
}

export interface RightPanelData {
  bookInfo: BookShortInfo[],
  totalPages: number
}

export interface MainPageModel {
  leftPanelData: LeftPanelData,
  rightPanelData: RightPanelData
}

@Injectable()
export class Repository {
  mainPageModel: MainPageModel;
  activePages: number[] = [1];
  isLoading: boolean;
  currentBookDetails: BookDetailedInfo;
  tags: Tag[];
  tagSaveErrors: string;
  tagTableErrors: string;
  authors: Author[];
  authorSaveErrors: string;
  authorTableErrors: string;
  bookMetadata: BookMetadata = { authors: [], categories: [], languages: [], tags: [] };
  editedBook: BookCreateOrEdit = new BookCreateOrEdit();

  constructor(private httpClient: HttpClient, private router: Router) {
  }

  getBookListWithMetadata(category: string = "all", page: number = 1, search: string = "") {
    const url = `/api/books/list/${category || "all"}/page${page || 1}` + (search ? search : "");
    this.isLoading = true;
    this.httpClient.get<MainPageModel>(url).subscribe(
      result => {
        this.mainPageModel = result;
        this.activePages = [page];
        this.isLoading = false;
      });
  }

  getNextBookInfo(category: string = "all", currentPage: number = 1, search: string = "") {
    const url = `/api/books/listAppend?category=${category}&page=${currentPage + 1}` + (search ? '&' + search.substring(1) : "")
    this.isLoading = true;
    this.httpClient.get<BookShortInfo[]>(url).subscribe(
      result => {
        this.mainPageModel.rightPanelData.bookInfo = this.mainPageModel.rightPanelData.bookInfo.concat(result);
        this.activePages.push(currentPage + 1);
        this.isLoading = false;
      });
  }

  getBookDetails(bookId: number) {
    if (!bookId) {
      throw new Error("invalid bookId");
    }
    this.httpClient.get<BookDetailedInfo>(`api/books/details/${bookId}`).subscribe(response => {
      this.currentBookDetails = response;
    });
  }

  getTags() {
    this.httpClient.get<Tag[]>('api/tags').subscribe(result => {
      this.tags = result;
    });
  }

  saveNewTag(newTag: Tag) {
    this.httpClient.post<any>('/api/tags', newTag).subscribe(
      (res) => {
        if (res.errors) {
          this.tagSaveErrors = res.errors;
        }
        else {
          newTag.tagId = res.tagId;
          this.tags.push(newTag);
        }
      },
      (er: HttpErrorResponse) => {
        this.tagSaveErrors = er.message;
      }
    );
  }

  updateTag(editedTag: Tag) {
    this.tagTableErrors = "";
    this.httpClient.put<any>(`/api/tags/${editedTag.tagId}`, editedTag).subscribe(
      (res) => {
        if (res.errors) {
          this.tagTableErrors = res.errors;
        }
        else {
          const index = this.tags.findIndex(t => t.tagId === editedTag.tagId);
          if (index >= 0) {
            this.tags[index] = editedTag;
          }
        }
      },
      (er: HttpErrorResponse) => {
        this.tagTableErrors = er.message;
      }
    );
  }

  deleteTag(tagId: number) {
    this.tagTableErrors = "";
    this.httpClient.delete<any>(`/api/tags/${tagId}`).subscribe(() => {
      this.tags = this.tags.filter(t => t.tagId !== tagId);
    },
      (er: HttpErrorResponse) => { this.tagTableErrors = er.message; });
  }

  getAuthors() {
    this.httpClient.get<Author[]>('api/authors').subscribe(result => {
      this.authors = result;
    });
  }

  saveNewAuthor(newAuthor: Author) {
    this.httpClient.post<any>('/api/authors', newAuthor).subscribe(
      (res) => {
        if (res.errors) {
          this.authorSaveErrors = res.errors;
        }
        else {
          newAuthor.authorId = res.authorId;
          this.authors.push(newAuthor);
        }
      },
      (er: HttpErrorResponse) => {
        this.authorSaveErrors = er.message;
      }
    );
  }

  updateAuthor(editedAuthor: Author) {
    this.authorTableErrors = "";
    this.httpClient.put<any>(`/api/authors/${editedAuthor.authorId}`, editedAuthor).subscribe(
      (res) => {
        if (res.errors) {
          this.authorTableErrors = res.errors;
        }
        else {
          const index = this.authors.findIndex(a => a.authorId === editedAuthor.authorId);
          if (index >= 0) {
            this.authors[index] = editedAuthor;
          }
        }
      },
      (er: HttpErrorResponse) => {
        this.authorTableErrors = er.message;
      }
    );
  }

  deleteAuthor(authorId: number) {
    this.authorTableErrors = "";
    this.httpClient.delete<any>(`/api/authors/${authorId}`).subscribe(() => {
      this.authors = this.authors.filter(a => a.authorId !== authorId);
    },
      (er: HttpErrorResponse) => { this.authorTableErrors = er.message; });
  }

  getMetadataForBook() {
    this.httpClient.get<BookMetadata>('/api/books/create').subscribe((result) => {
      this.bookMetadata = result;
    });
  }

  getBookForEditing(bookId: number) {
    this.httpClient.get<any>(`/api/books/edit/${bookId}`).subscribe((result) => {
      this.bookMetadata = { authors: result.authors, categories: result.categories, languages: result.languages, tags: result.tags };
      this.editedBook = new BookCreateOrEdit(result.bookId, result.name, result.description, result.pages, result.year, result.base64ImageSrc, result.selectedCategoryId,
        result.selectedLanguageId, result.selectedAuthorsId, result.selectedTagsId);
    });
  }

  saveBook(book: BookCreateOrEdit) {
    if (book.bookId) {//put
      this.httpClient.put<any>(`/api/books/${book.bookId}`, book).subscribe(
        (res) => {
          if (res.errors) {
            throw new Error(res.errors);
          }
          else {
            this.router.navigateByUrl('/');
          }
        }),
        (er: HttpErrorResponse) => {
          throw er;
        }
    }
    else {//post
      this.httpClient.post<any>('/api/books/', book).subscribe(
        (res) => {
          if (res.errors) {
            throw new Error(res.errors);
          }
          else {
            this.router.navigateByUrl('/');
          }
        }),
        (er: HttpErrorResponse) => {
          throw er;
        }
    }
  }

  deleteBook(bookId: number) {
    this.httpClient.delete<any>(`api/books/${bookId}`).subscribe((res) => {
      this.getBookListWithMetadata();
    });
  }
}
