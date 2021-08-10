import { Injectable } from "@angular/core";
import { ActivatedRoute, NavigationEnd, Router } from '@angular/router';
import { Filter } from "./filter.model";
import { filter, map } from 'rxjs/operators';
import { Repository } from "./repository";
@Injectable()
export class NavigationService {
  filter: Filter = new Filter();
  currentPage: number = 1;

  constructor(private repo: Repository, private router: Router, private activeRoute: ActivatedRoute) {
    router.events.pipe(
      filter(event => event instanceof NavigationEnd)
    ).subscribe(navEnd => this.handleNavigationChange(navEnd));
  }

  handleNavigationChange(navEnd): void {
    let active = this.router.routerState.root.children[0].snapshot.firstChild;
    if (active.url.length === 0) {
      this.filter.reset();
      this.currentPage = 1;
      this.repo.getBookListWithMetadata(); //get with default params
    }
    else if (active.url.length > 1 && active.url[0].path === "books" && active.url[1].path === "list") {
      let category = active.params["category"] || "all";
      this.filter.category = category;
      let pageStr: string = active.params["page"];
      let page = 1;
      if (pageStr) {
        this.currentPage = page = parseInt(pageStr.slice(4)) || 1;
      }
      let search = this.getCurrentSearch();
      this.repo.getBookListWithMetadata(category, page, search);
    }
  }

  getCurrentSearch(): string {
    let search = "";
    let query = this.activeRoute.snapshot.queryParams;
    if (query) {
      const queryAr = [];
      for (let key in query) {
        search = key;
        search += '=';
        search += query[key];
        queryAr.push(search);
      }
      search = queryAr.join('&');
    }
    return search ? "?" + search: "";
  }
}
