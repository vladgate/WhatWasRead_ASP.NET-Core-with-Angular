import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { BookDetailsComponent } from './home/book-details/book-details.component';
import { HomeComponent } from './home/home.component';
import { MainLayoutComponent } from './home/main-layout/main-layout.component';

const routes: Routes = [
  { path: "admin", loadChildren: './admin/admin.module#AdminModule' },
  {
    path: "", component: MainLayoutComponent, children: [
      { path: "books/details/:id", component: BookDetailsComponent, pathMatch: "full" },
      { path: "books/list/:category/:page", component: HomeComponent },
      { path: "", component: HomeComponent, pathMatch: "full" },
      { path: "**", redirectTo: "/" }
    ]
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
