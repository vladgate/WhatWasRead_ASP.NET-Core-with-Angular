import { CommonModule } from "@angular/common";
import { NgModule } from "@angular/core";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { RouterModule } from "@angular/router";
import { LoaderComponent } from "./components/loader/loader.component";

@NgModule({
  declarations: [
    LoaderComponent
    ],
  imports: [
    RouterModule,
    CommonModule,
    FormsModule,
    ReactiveFormsModule],
  exports: [LoaderComponent, CommonModule, RouterModule, FormsModule, ReactiveFormsModule]
})
export class SharedModule {
}
