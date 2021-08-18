import { NgModule } from "@angular/core";
import { BrowserModule } from "@angular/platform-browser";
import { NavigationService } from "../models/navigation.service";
import { Repository } from "../models/repository";
import { SharedModule } from "../shared/shared.module";
import { HomeComponent } from "./home.component";
import { BookInfoComponent } from "./structure/book-info/book-info.component";
import { BookListComponent } from "./structure/book-list/book-list.component";
import { BookRowComponent } from "./structure/book-row/book-row.component";
import { LeftPanelComponent } from "./structure/left-panel/left-panel.component";
import { PaginationComponent } from "./structure/pagination/pagination.component";
import { RightPanelComponent } from "./structure/right-panel/right-panel.component";
import { MainLayoutComponent } from "./main-layout/main-layout.component";

@NgModule({
  declarations: [
    MainLayoutComponent,
    HomeComponent,
    LeftPanelComponent,
    RightPanelComponent,
    BookInfoComponent,
    BookListComponent,
    BookRowComponent,
    PaginationComponent
  ],
  imports: [BrowserModule, SharedModule],
  providers: [Repository, NavigationService],
  exports:[]
})

export class MainPageModule {
}
