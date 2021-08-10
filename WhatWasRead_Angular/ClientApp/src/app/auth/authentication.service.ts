import { Injectable } from "@angular/core";
import { Observable, of } from "rxjs";
import { map, catchError } from 'rxjs/operators';
import { Router } from "@angular/router";
import { HttpClient } from "@angular/common/http";

@Injectable()
export class AuthenticationService {

  constructor(private http: HttpClient, private router: Router) { }

  authenticated: boolean = false;
  name: string;
  password: string;
  callbackUrl: string;

  login(): Observable<any> {
    this.authenticated = false;
    return this.http.post<boolean>("/api/account/login", { name: this.name, password: this.password }).pipe(
      map(response => {
        if (response) {
          this.authenticated = true;
          this.password = null;
          this.router.navigateByUrl(this.callbackUrl || "/admin/overview");
        }
        return this.authenticated;
      }),
      catchError(_e => {
        this.authenticated = false;
        return of(false);
      }));
  }

  logout() {
    this.authenticated = false;
    this.http.post("/api/account/logout", null).subscribe(response => { });
    this.router.navigateByUrl("/admin/login");
  }
}
