import { Component } from '@angular/core';
import { AuthService } from './services/auth-service.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent {
  title = 'SistemaCitasView';

    isLoggedIn = false;

  constructor(public authService: AuthService) {}

  ngOnInit() {
    this.isLoggedIn = !!this.authService.getToken();
  }
}
