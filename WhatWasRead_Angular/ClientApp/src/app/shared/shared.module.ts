import { CommonModule } from "@angular/common";
import { NgModule } from "@angular/core";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { RouterModule } from "@angular/router";
import { BookDetailsComponent } from "./components/book-details/book-details.component";
import { LoaderComponent } from "./components/loader/loader.component";

@NgModule({
  declarations: [
    LoaderComponent,
    BookDetailsComponent
    ],
  imports: [
    RouterModule,
    CommonModule,
    FormsModule,
    ReactiveFormsModule],
  exports: [LoaderComponent, CommonModule, RouterModule, FormsModule, ReactiveFormsModule, BookDetailsComponent],
  providers: []
})
export class SharedModule {
}
