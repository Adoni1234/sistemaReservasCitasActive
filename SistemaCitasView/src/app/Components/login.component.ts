import { Component } from '@angular/core';
import { LoginServicesService } from '../services/login-services.service';
import { Router } from '@angular/router';

@Component({
  selector: 'LoginComponent',
  template: `
  <div class="min-h-screen flex items-center justify-center bg-gradient-to-br from-blue-100 to-blue-300">
    <div class="bg-white rounded-2xl shadow-lg p-8 w-full max-w-md">
      
      <!-- Logo -->
      <div class="flex flex-col items-center mb-6">
        <img src="assets/imagenes/logo.png" alt="Logo" class="w-20 mb-3" />
        <h1 class="text-2xl font-bold text-blue-600">Iniciar sesión</h1>
      </div>

      <form (ngSubmit)="onSubmit()" #formRef="ngForm" novalidate class="space-y-4">

        <div>
          <label class="block text-sm font-medium text-gray-700">Usuario</label>
          <input
            name="usuarioNombre"
            [(ngModel)]="usuarioNombre"
            #usuarioInput="ngModel"
            required
            minlength="4"
            placeholder="Nombre de usuario"
            class="w-full mt-1 px-4 py-2 border rounded-lg focus:ring-2 focus:ring-blue-400 outline-none"
            [class.border-red-500]="submitted && usuarioInput.invalid"
          />
          <div *ngIf="submitted && usuarioInput.errors" class="text-red-600 text-sm mt-1">
            <div *ngIf="usuarioInput.errors['required']">El usuario es obligatorio.</div>
            <div *ngIf="usuarioInput.errors['minlength']">Mínimo 4 caracteres.</div>
          </div>
        </div>

        <div>
          <label class="block text-sm font-medium text-gray-700">Contraseña</label>
          <input
            type="password"
            name="password"
            [(ngModel)]="password"
            #passwordInput="ngModel"
            required
            minlength="6"
            placeholder="••••••••"
            class="w-full mt-1 px-4 py-2 border rounded-lg focus:ring-2 focus:ring-blue-400 outline-none"
            [class.border-red-500]="submitted && passwordInput.invalid"
          />
          <div *ngIf="submitted && passwordInput.errors" class="text-red-600 text-sm mt-1">
            <div *ngIf="passwordInput.errors['required']">La contraseña es obligatoria.</div>
            <div *ngIf="passwordInput.errors['minlength']">Mínimo 6 caracteres.</div>
          </div>
        </div>

        <button
          type="submit"
          class="w-full bg-blue-600 hover:bg-blue-700 text-white font-semibold py-2 rounded-lg transition"
          [disabled]="loading"
        >
          {{ loading ? 'Ingresando...' : 'Iniciar sesión' }}
        </button>
      </form>

      <p *ngIf="errorMessage" class="mt-4 text-center text-sm text-red-600">{{ errorMessage }}</p>
      <p *ngIf="successMessage" class="mt-4 text-center text-sm text-green-600">{{ successMessage }}</p>

      <!-- Texto para registro -->
      <p class="mt-6 text-center text-sm text-gray-600">
        ¿No tienes cuenta?
        <a routerLink="/registro" class="text-blue-600 hover:underline">Regístrate</a>
      </p>
    </div>
  </div>
  `
})
export class LoginComponent {
  usuarioNombre: string = '';
  password: string = '';

  submitted = false;
  loading = false;
  errorMessage = '';
  successMessage = '';

  constructor(private api: LoginServicesService, private router: Router) {}

  onSubmit() {
    this.submitted = true;
    this.errorMessage = '';
    this.successMessage = '';

    if (!this.usuarioNombre || this.usuarioNombre.length < 4 || !this.password || this.password.length < 6) {
      return;
    }

    this.loading = true;

    const loginData = {
      usuarioNombre: this.usuarioNombre,
      password: this.password
    };

    this.api.loginAuth(loginData).subscribe({
      next: (res) => {
        if (res.success && res.data?.token) {
          sessionStorage.setItem('token', res.data.token);
          this.successMessage = 'Inicio de sesión exitoso';
          setTimeout(() => {
            this.router.navigate(['/dashboard']);
          }, 1000);
        } else {
          this.errorMessage = res.message || 'Error en el inicio de sesión';
        }
        this.loading = false;
      },
      error: (err) => {
        this.errorMessage = 'Error en el servidor o credenciales inválidas';
        console.error(err);
        this.loading = false;
      }
    });
  }
}
