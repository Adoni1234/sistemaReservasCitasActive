import { Component } from "@angular/core";
import { LoginServicesService } from "../services/login-services.service";
import { Usuario } from "../Models/Usuario";

@Component({
  selector: "RegisterComponent",
  template: ` 
    <div class="min-h-screen flex items-center justify-center bg-gradient-to-br from-green-100 to-green-300">
      <div class="bg-white rounded-2xl shadow-lg p-8 w-full max-w-md">
        <!-- Logo -->
        <div class="flex flex-col items-center">
          <img src="assets/imagenes/logo.png" alt="Logo" class="w-20 mb-3" />
          <h1 class="text-2xl font-bold text-green-600">Crear cuenta</h1>
        </div>

        <!-- Formulario -->
        <form (ngSubmit)="onSubmit()" #formRef="ngForm" novalidate class="mt-6 space-y-4">

          <div>
            <label class="block text-sm font-medium text-gray-700">Usuario</label>
            <input
              name="UsuarioNombre"
              [(ngModel)]="UsuarioNombre"
              #usuarioInput="ngModel"
              required
              minlength="4"
              placeholder="Nombre de usuario"
              class="w-full mt-1 px-4 py-2 border rounded-lg focus:ring-2 focus:ring-green-400 outline-none"
              [class.border-red-500]="submitted && usuarioInput.invalid"
            />
            <div *ngIf="submitted && usuarioInput.errors" class="text-red-600 text-sm mt-1">
              <div *ngIf="usuarioInput.errors['required']">El usuario es obligatorio.</div>
              <div *ngIf="usuarioInput.errors['minlength']">Mínimo 4 caracteres.</div>
            </div>
          </div>

          <div>
            <label class="block text-sm font-medium text-gray-700">Correo electrónico</label>
            <input
              type="email"
              name="Email"
              [(ngModel)]="Email"
              #emailInput="ngModel"
              required
              email
              placeholder="ejemplo@correo.com"
              class="w-full mt-1 px-4 py-2 border rounded-lg focus:ring-2 focus:ring-green-400 outline-none"
              [class.border-red-500]="submitted && emailInput.invalid"
            />
            <div *ngIf="submitted && emailInput.errors" class="text-red-600 text-sm mt-1">
              <div *ngIf="emailInput.errors['required']">El email es obligatorio.</div>
              <div *ngIf="emailInput.errors['email']">Formato de email inválido.</div>
            </div>
          </div>

          <div>
            <label class="block text-sm font-medium text-gray-700">Contraseña</label>
            <input
              type="password"
              name="Password"
              [(ngModel)]="Password"
              #passwordInput="ngModel"
              required
              minlength="6"
              placeholder="••••••••"
              class="w-full mt-1 px-4 py-2 border rounded-lg focus:ring-2 focus:ring-green-400 outline-none"
              [class.border-red-500]="submitted && passwordInput.invalid"
            />
            <div *ngIf="submitted && passwordInput.errors" class="text-red-600 text-sm mt-1">
              <div *ngIf="passwordInput.errors['required']">La contraseña es obligatoria.</div>
              <div *ngIf="passwordInput.errors['minlength']">Mínimo 6 caracteres.</div>
            </div>
          </div>

          <button
            type="submit"
            class="w-full bg-green-500 hover:bg-green-600 text-white font-semibold py-2 rounded-lg transition"
            [disabled]="loading"
          >
            {{ loading ? 'Registrando...' : 'Registrarse' }}
          </button>
        </form>

        <!-- Mensaje de éxito -->
        <p *ngIf="successMessage" class="mt-4 text-center text-sm text-green-600 font-semibold">{{ successMessage }}</p>

        <!-- Mensaje de error -->
        <p *ngIf="errorMessage" class="mt-4 text-center text-sm text-red-600">{{ errorMessage }}</p>

        <!-- Login link -->
        <p class="mt-4 text-center text-sm text-gray-600">
          ¿Ya tienes cuenta?
          <a routerLink="/" class="text-green-500 hover:underline">Inicia sesión</a>
        </p>
      </div>
    </div>
  `
})
export class RegisterComponent {
  UsuarioNombre: string = '';
  Email: string = '';
  Password: string = '';
  Rol: number = 1;

  submitted = false;
  loading = false;
  errorMessage = '';
  successMessage = '';

  constructor(private api: LoginServicesService) {}

  onSubmit() {
    this.submitted = true;
    this.errorMessage = '';
    this.successMessage = '';

    if (
      !this.UsuarioNombre || this.UsuarioNombre.length < 4 ||
      !this.Email || !this.validarEmail(this.Email) ||
      !this.Password || this.Password.length < 6
    ) {
      return;
    }

    this.loading = true;

    const nuevoUsuario: Usuario = {
      UsuarioNombre: this.UsuarioNombre,
      Email: this.Email,
      Password: this.Password,
      Rol: this.Rol
    };

    this.api.createUser(nuevoUsuario).subscribe({
      next: () => {
        this.successMessage = 'Usuario registrado con éxito';
        this.UsuarioNombre = '';
        this.Email = '';
        this.Password = '';
        this.submitted = false;
        this.loading = false;
      },
      error: (err) => {
        this.errorMessage = 'Error al registrar usuario';
        console.error(err);
        this.loading = false;
      }
    });
  }

  validarEmail(email: string): boolean {
    const re = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    return re.test(email);
  }
}
