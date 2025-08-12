import { Component } from '@angular/core';
import { AuthService } from '../../services/auth-service.service';
@Component({
    selector: 'LayoutComponent',
    template: `
    <div class="h-screen w-64 bg-gray-800 text-white flex flex-col">

      <!-- Logo y usuario -->
      <div class="flex items-center px-4 py-5 border-b border-gray-700">
        <img src="assets/imagenes/logo.png" alt="Logo" class="w-10 h-10 rounded-full mr-3">
        <div>
          <h1 class="font-bold text-lg">Mi Sistema</h1>
          <p class="text-sm text-gray-300">{{ userName }}</p>
        </div>
      </div>

      <!-- Botones de menú -->
      <nav class="flex-1 mt-4">
        <button
          class="flex items-center w-full px-4 py-3 hover:bg-gray-700 transition-colors"
          routerLink="/userManager"
        >
          <i class="fas fa-users mr-3"></i>
          <span>Gestión Usuario</span>
        </button>

        <button
          class="flex items-center w-full px-4 py-3 hover:bg-gray-700 transition-colors"
          routerLink="/gestion-fechas"
        >
          <i class="fas fa-calendar-alt mr-3"></i>
          <span>Gestión Fechas Hábiles</span>
        </button>

        <button
          class="flex items-center w-full px-4 py-3 hover:bg-gray-700 transition-colors mt-auto"
          (click)="logout()"
        >
          <i class="fas fa-sign-out-alt mr-3"></i>
          <span>Salir</span>
        </button>
      </nav>
    </div>
  `
})
export class LayoutComponent {
    userName: string = 'Usuario';

constructor(private authService: AuthService) {
  const token = this.authService.getToken();
  if (token) {
    const decoded = this.authService.decodeToken();
    console.log(decoded);
    if (decoded && decoded['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name']) {
      this.userName = decoded['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name'];
    } else {
      this.userName = 'Invitado';
    }
  } else {
    this.userName = 'Invitado';
  }

}


    logout() {
        this.authService.removeToken();
        window.location.href = '/login';
    }
}
