import { Component, OnInit } from '@angular/core';
import { AuthenticationService } from '../../auth/authentication.service';

@Component({
  selector: 'app-admin-layout',
  templateUrl: './admin-layout.component.html',
  styleUrls: ['./admin-layout.component.css']
})
export class AdminLayoutComponent implements OnInit {

  constructor(private authService: AuthenticationService) { }

  ngOnInit() {
  }

}
