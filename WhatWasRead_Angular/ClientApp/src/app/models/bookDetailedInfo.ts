import { Tag } from "./tag.model";

export class BookDetailedInfo {
  constructor(public bookId: number, public authorsOfBooks: string, public base64ImageSrc: string, public bookTags: Tag[],
    public category: string, public description: string, public language: string, public name: string, public pages: number, public year: number) {
  }
}
