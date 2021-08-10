
export class Filter {
  category?: string = "all";
  language?: string = "";
  author?: string = "";
  tag?: string = "";
  minPages: number = 0;
  maxPages: number = 0;

  reset() {
    this.category = "all";
    this.language = "";
    this.author = "";
    this.tag = "";
    this.minPages = 0;
    this.maxPages = 0;
  }
}
