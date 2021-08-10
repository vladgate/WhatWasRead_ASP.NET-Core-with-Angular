import { NgModule } from "@angular/core";
import { SharedModule } from "../shared/shared.module";
import { AuthenticationGuard } from "./authentication.guard";
import { AuthenticationService } from "./authentication.service";
import { LoginPageComponent } from "./login-page/login-page.component";

@NgModule({
  imports: [SharedModule],
  declarations: [LoginPageComponent],
  providers: [AuthenticationService, AuthenticationGuard],
  exports: [LoginPageComponent]
})
export class AuthModule {

}
