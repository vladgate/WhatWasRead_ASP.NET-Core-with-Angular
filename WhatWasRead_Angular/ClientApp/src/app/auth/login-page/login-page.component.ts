import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthenticationService } from '../authentication.service';

@Component({
  selector: 'app-login-page',
  templateUrl: './login-page.component.html',
  styleUrls: ['./login-page.component.css']
})
export class LoginPageComponent implements OnInit {
  form: FormGroup;
  showError: boolean = false;
  submitted: boolean = false;

  constructor(public authService: AuthenticationService, private router: Router) { }

  ngOnInit() {
    this.form = new FormGroup({
      username: new FormControl(null, [
        Validators.required]),
      password: new FormControl(null, [
        Validators.required,
        Validators.minLength(6)])
    })

  }

  submit() {
    if (this.form.invalid) {
      return;
    }
    this.showError = false;
    this.submitted = true;
    this.authService.login(this.form.value.username, this.form.value.password).subscribe(() => {
      this.form.get('password').setValue("");
      this.submitted = false;
      if (this.authService.authenticated) {
        this.router.navigate(['/admin', 'overview']);
      }
      else {
        this.showError = true;
      }
    },
      () => {
        this.submitted = false;
      });
  }
}
