import { Injectable } from "@angular/core";
import { Router, ActivatedRouteSnapshot, } from "@angular/router";
import { AuthenticationService } from "./authentication.service";

@Injectable()
export class AuthenticationGuard {
  constructor(private router: Router, private authService: AuthenticationService) { }

  canActivateChild(route: ActivatedRouteSnapshot): boolean {
    if (this.authService.authenticated) {
      return true;
    } else {
      this.authService.callbackUrl = route.url.toString();
      this.router.navigateByUrl("/admin/login");
      return false;
    }
  }
}
