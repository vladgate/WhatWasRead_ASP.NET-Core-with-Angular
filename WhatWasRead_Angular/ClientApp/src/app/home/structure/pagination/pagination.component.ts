import { Component, OnChanges, SimpleChanges } from "@angular/core";
import { NavigationService } from "../../../models/navigation.service";
import { Repository } from "../../../models/repository";

@Component({
  selector: 'pagination',
  templateUrl: 'pagination.component.html',
  styleUrls: ['pagination.component.css']
})
export class PaginationComponent {

  constructor(private repo: Repository, private navigationService: NavigationService) {

  }

  get pages(): number[] {
    const totalPages = this.repo.mainPageModel.rightPanelData.totalPages;
    if (totalPages > 0) {
      const ar: Array<number> = [];
      for (let i = 0; i < totalPages; i++) {
        ar.push(i + 1);
      }
      return ar;
    }
    else {
      return [];
    }
  }

  get currentCategory(): string {
    return this.navigationService.filter.category;
  }

  get totalPages(): number {
    return this.repo.mainPageModel.rightPanelData.totalPages;
  }

  pageIsActive(value: number): boolean {
    return this.repo.activePages.indexOf(value) >= 0;
  }

  showMoreBooks() {
    const category = this.navigationService.filter.category;
    const currentpage = this.navigationService.currentPage;
    const search = this.navigationService.getCurrentSearch();
    this.repo.getNextBookInfo(category, currentpage, search);
    this.navigationService.currentPage = currentpage + 1;
  }
}
