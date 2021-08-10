import { Component, OnInit } from "@angular/core";
import { Router } from '@angular/router';
import { Author } from "../../../models/author.model";
import { Language } from "../../../models/language.model";
import { NavigationService } from "../../../models/navigation.service";
import { Repository } from "../../../models/repository";

@Component({
  selector: 'left-panel',
  templateUrl: 'left-panel.component.html',
  styleUrls: ['left-panel.component.css']
})
export class LeftPanelComponent implements OnInit {

  LANGUAGE_QUERY_WORD = "lang";
  AUTHOR_QUERY_WORD = "author";
  PAGES_QUERY_WORD = "pages";

  constructor(private repo: Repository, private router: Router, private navigationService: NavigationService) {
  }

  ngOnInit(): void {
    const model = this.repo.mainPageModel.leftPanelData;
    model.minPagesActual = model.minPagesActual || model.minPagesExpected || 0;
    model.maxPagesActual = model.maxPagesActual || model.maxPagesExpected || 0;
  }

  get categories() {
    return this.repo.mainPageModel.leftPanelData.categories;
  }

  get languages() {
    return this.repo.mainPageModel.leftPanelData.languages;
  }

  get authors() {
    return this.repo.mainPageModel.leftPanelData.authors;
  }

  get tags() {
    return this.repo.mainPageModel.leftPanelData.tags;
  }

  onLanguageAnchorClick($event, language: Language) {
    $event.preventDefault();
    this.router.navigateByUrl(language.link);
  }

  onAuthorAnchorClick($event, author: Author) {
    $event.preventDefault();
    this.router.navigateByUrl(author.link);
  }

  onApplyFilterButtonClick() {
    const model = this.repo.mainPageModel.leftPanelData;
    const queryStringAr = [];
    const checkedLanguages = model.languages.filter(item => item.checked);
    const ar = [];
    for (let i = 0; i < checkedLanguages.length; i++) {
      ar.push(checkedLanguages[i].nameForLinks);
    }
    let qLang = "";
    if (ar.length > 0) {
      qLang = this.LANGUAGE_QUERY_WORD + "=" + ar.join(",");
      queryStringAr.push(qLang);
    }
    const checkedAuthors = model.authors.filter(item => item.checked);
    ar.length = 0;
    for (let i = 0; i < checkedAuthors.length; i++) {
      ar.push(checkedAuthors[i].authorId);
    }
    var qAuthors = "";
    if (ar.length > 0) {
      qAuthors = this.AUTHOR_QUERY_WORD + "=" + ar.join(",");
      queryStringAr.push(qAuthors);
    }
    const minExpected = model.minPagesExpected;
    const maxExpected = model.maxPagesExpected;
    const minActual = model.minPagesActual;
    const maxActual = model.maxPagesActual;

    if (minActual && maxActual && (minActual !== minExpected || maxActual !== maxExpected) && minActual <= maxActual) /*correct values*/ {
      const qPages = this.PAGES_QUERY_WORD + "=" + minActual + "-" + maxActual;
      queryStringAr.push(qPages);
    }
    const qString = queryStringAr.join("&");
    const url = `/books/list/${this.navigationService.filter.category}/page1` + (qString ? "?" + qString : "");
    this.router.navigateByUrl(url);
  }

  onResetFilterButtonClick() {
    this.navigationService.filter.reset();
    this.router.navigateByUrl(`/books/list/${this.navigationService.filter.category}/page${this.navigationService.currentPage}`);
  }
}
