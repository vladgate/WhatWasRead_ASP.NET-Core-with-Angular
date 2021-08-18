import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";
import { SharedModule } from "../shared/shared.module";
import { AdminOverviewComponent } from './admin-overview/admin-overview.component';
import { AuthenticationGuard } from "../auth/authentication.guard";
import { AuthModule } from "../auth/auth.module";
import { LoginPageComponent } from "../auth/login-page/login-page.component";
import { AdminLayoutComponent } from './admin-layout/admin-layout.component';
import { TagListComponent } from './tag-list/tag-list.component';
import { AuthorListComponent } from './author-list/author-list.component';
import { BookCreateOrEditComponent } from './book-createOrEdit/book-createOrEdit.component';

const routes: Routes = [
  { path: "login", component: LoginPageComponent },
  {
    path: "", component: AdminLayoutComponent, canActivateChild: [AuthenticationGuard],
    children: [
      { path: "books/edit/:id", component: BookCreateOrEditComponent, pathMatch: "full" },
      { path: "books/create", component: BookCreateOrEditComponent, pathMatch: "full" },
      { path: "tags", component: TagListComponent },
      { path: "authors", component: AuthorListComponent },
      { path: "overview", component: AdminOverviewComponent },
      { path: "", component: AdminOverviewComponent }
    ]
  }
];

@NgModule({
  imports: [SharedModule, RouterModule.forChild(routes), AuthModule],
  declarations: [AdminOverviewComponent, AdminLayoutComponent, TagListComponent, AuthorListComponent, BookCreateOrEditComponent]
})
export class AdminModule {

}
