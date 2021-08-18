import { Component } from "@angular/core";
import { Router } from "@angular/router";
import { AuthenticationService } from "../../auth/authentication.service";
import { Location} from '@angular/common';

@Component({
  selector: "app-layout",
  templateUrl: "./main-layout.component.html",
  styleUrls: ["./main-layout.component.css"],
  providers: [Location]
})
export class MainLayoutComponent {
  constructor(private authService: AuthenticationService, private location: Location) { }

  wantAdmin() {
    this.authService.callbackUrl = this.location.path();
  }
}
