import { Component, OnInit } from '@angular/core';
import { NavigationService } from '../models/navigation.service';//for initialization navService
import { Repository } from '../models/repository';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls:['./home.component.css']
})
export class HomeComponent implements OnInit {

  constructor(private repo: Repository, private navigationService: NavigationService) {
  }

  ngOnInit(): void {
  }

  get isLoading(): boolean {
    return this.repo.isLoading;
  }

  get init(): boolean {
    return <boolean><any>this.repo.mainPageModel;
  }
}
