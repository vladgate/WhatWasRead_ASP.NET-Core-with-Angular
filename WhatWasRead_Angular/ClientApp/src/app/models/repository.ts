import { Injectable } from '@angular/core';
import { BookShortInfo } from './bookShortInfo.model';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Author } from './author.model';
import { Category } from './category.model';
import { Language } from './language.model';
import { Tag } from './tag.model';
import { BookDetailedInfo } from './bookDetailedInfo';

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


  constructor(private httpClient: HttpClient) {
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
    this.isLoading = true;
    this.httpClient.get<BookDetailedInfo>(`api/books/details/${bookId}`).subscribe(response => {
      this.currentBookDetails = response;
      this.isLoading = false;
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
        newTag.tagId = res.tagId;
        this.tags.push(newTag);
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
        const index = this.tags.findIndex(t => t.tagId === editedTag.tagId);
        if (index >= 0) {
          this.tags[index] = editedTag;
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
}
