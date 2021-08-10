import { CommonModule } from "@angular/common";
import { NgModule } from "@angular/core";
import { FormsModule } from "@angular/forms";
import { RouterModule } from "@angular/router";
import { LoaderComponent } from "./components/loader/loader.component";

@NgModule({
  declarations: [
    LoaderComponent
    ],
  imports: [
    RouterModule,
    CommonModule,
    FormsModule],
  exports: [LoaderComponent, CommonModule, RouterModule, FormsModule]
})
export class SharedModule {
}
