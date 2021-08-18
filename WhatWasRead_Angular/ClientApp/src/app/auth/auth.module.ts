import { NgModule } from "@angular/core";
import { SharedModule } from "../shared/shared.module";
import { AuthenticationGuard } from "./authentication.guard";
import { LoginPageComponent } from "./login-page/login-page.component";

@NgModule({
  imports: [SharedModule],
  declarations: [LoginPageComponent],
  providers: [AuthenticationGuard],
  exports: [LoginPageComponent]
})
export class AuthModule {
}
