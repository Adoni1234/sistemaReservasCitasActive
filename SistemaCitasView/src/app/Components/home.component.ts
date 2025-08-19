import { Component } from '@angular/core';
import { AuthService } from '../services/auth-service.service';

@Component({
  selector: 'HomeComponent',
  template: `
    <div class="flex items-center justify-center h-screen bg-gray-100">
      <div class="text-center bg-white shadow-xl rounded-3xl p-16 max-w-3xl w-full">
        <!-- Logo -->
        <div class="flex justify-center mb-8">
          <img 
            src="assets/imagenes/logo.png" 
            alt="Logo" 
            class="w-32 h-32 rounded-full shadow-md border border-gray-200"
          />
        </div>

        <!-- Bienvenida -->
        <h1 class="text-5xl font-extrabold text-gray-800 mb-6">
          ¡Bienvenido, <span class="text-indigo-600">{{ userName }}</span>!
        </h1>
        
        <p class="text-xl text-gray-600">
          Estás en el sistema <span class="font-semibold text-indigo-600">AgendaCitas</span>
        </p>
      </div>
    </div>
  `
})
export class HomeComponent {
  userName: string = 'Usuario';
  role: string = '';

  constructor(private authService: AuthService) {
    const token = this.authService.getToken();
    if (token) {
      const decoded = this.authService.decodeToken();
      if (
        decoded &&
        decoded['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name']
      ) {
        this.userName =
          decoded['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name'];
        this.role =
          decoded[
            'http://schemas.microsoft.com/ws/2008/06/identity/claims/role'
          ];
      } else {
        this.userName = 'Invitado';
      }
    } else {
      this.userName = 'Invitado';
    }
  }
}
