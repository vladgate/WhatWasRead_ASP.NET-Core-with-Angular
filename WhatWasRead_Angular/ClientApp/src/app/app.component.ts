import { Component } from '@angular/core';
import { Repository } from './models/repository';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html'
})
export class AppComponent {
  constructor(private repo: Repository) { }

  get loading(): boolean {
    return this.repo.isLoading;
  }
}
