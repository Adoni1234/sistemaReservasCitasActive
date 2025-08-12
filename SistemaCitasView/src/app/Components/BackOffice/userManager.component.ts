import { Component, OnInit } from '@angular/core';
import { UsuarioGet } from '../../Models/UsuarioGet';
import { UserManagerService } from '../../services/backOffice/user-manager.service';

@Component({
  selector: 'userManagerComponent',
  template: `
    <div class="p-6">

      <div class="flex justify-between items-center mb-4">
        <h1 class="text-2xl font-bold">Gestión de Usuarios</h1>
        <button
          (click)="abrirFormularioCrear()"
          class="bg-green-600 hover:bg-green-700 text-white py-2 px-4 rounded"
        >
          Crear Usuario
        </button>
      </div>

      <!-- Formulario Crear/Editar Usuario -->
      <form
        *ngIf="mostrarFormulario"
        (ngSubmit)="submitFormulario()"
        class="mb-6 bg-white shadow p-4 rounded flex flex-col gap-4 w-full max-w-lg"
      >
        <input
          [(ngModel)]="usuarioForm.usuarioNombre"
          name="usuarioNombre"
          placeholder="Nombre de usuario"
          class="border p-2 rounded"
          required
        />
        <input
          [(ngModel)]="usuarioForm.password"
          name="password"
          type="password"
          placeholder="Contraseña"
          class="border p-2 rounded"
          [required]="!modoEdicion"
        />
        <input
          [(ngModel)]="usuarioForm.email"
          name="email"
          type="email"
          placeholder="Correo electrónico"
          class="border p-2 rounded"
          required
          [readonly]="modoEdicion"
        />
        <input
          [(ngModel)]="usuarioForm.rol"
          name="rol"
          type="number"
          placeholder="Rol"
          class="border p-2 rounded"
          required
        />
        <div class="flex gap-2">
          <button
            type="submit"
            class="bg-blue-500 hover:bg-blue-600 text-white py-2 px-4 rounded flex-1"
          >
            {{ modoEdicion ? 'Actualizar' : 'Crear' }}
          </button>
          <button
            type="button"
            (click)="cancelarFormulario()"
            class="bg-gray-400 hover:bg-gray-500 text-white py-2 px-4 rounded flex-1"
          >
            Cancelar
          </button>
        </div>
      </form>

      <!-- Tabla de usuarios -->
      <table class="table-auto w-full bg-white shadow rounded overflow-hidden">
        <thead class="bg-gray-800 text-white">
          <tr>
            <th class="px-4 py-2 text-left">Usuario</th>
            <th class="px-4 py-2 text-left">Email</th>
            <th class="px-4 py-2 text-left">Rol</th>
            <th class="px-4 py-2 text-center">Acciones</th>
          </tr>
        </thead>
        <tbody>
          <tr *ngFor="let user of dataUser" class="border-b">
            <td class="px-4 py-2">{{ user.usuarioNombre }}</td>
            <td class="px-4 py-2">{{ user.email }}</td>
            <td class="px-4 py-2">{{ user.rol === 0 ? 'Admin' : 'Usuario' }}</td>
            <td class="px-4 py-2 flex gap-2 justify-center">
              <button
                (click)="abrirFormularioEditar(user)"
                class="bg-yellow-500 hover:bg-yellow-600 text-white px-3 py-1 rounded"
              >
                <i class="fas fa-edit"></i>
              </button>
              <button
                (click)="eliminarUsuario(user)"
                class="bg-red-500 hover:bg-red-600 text-white px-3 py-1 rounded"
              >
                <i class="fas fa-trash"></i>
              </button>
            </td>
          </tr>
        </tbody>
      </table>
    </div>
  `,
})
export class UserManagerComponent implements OnInit {
  dataUser: UsuarioGet[] = [];
  mostrarFormulario = false;
  modoEdicion = false; // false = crear, true = editar

  usuarioForm: UsuarioGet = {
    id: 0,
    usuarioNombre: '',
    password: '',
    email: '',
    rol: 0,
  };

  constructor(private userManagerService: UserManagerService) {}

  ngOnInit() {
    this.cargarUsuarios();
  }

  cargarUsuarios() {
    this.userManagerService.GetAllUser().subscribe({
      next: (res) => (this.dataUser = res),
      error: (err) => console.error('Error cargando usuarios', err),
    });
  }

  abrirFormularioCrear() {
    this.modoEdicion = false;
    this.usuarioForm = {
      id: 0,
      usuarioNombre: '',
      password: '',
      email: '',
      rol: 0,
    };
    this.mostrarFormulario = true;
  }

  abrirFormularioEditar(user: UsuarioGet) {
    this.modoEdicion = true;
    this.usuarioForm = { ...user, password: '' }; // No mostramos la password
    this.mostrarFormulario = true;
  }

  cancelarFormulario() {
    this.mostrarFormulario = false;
  }

  submitFormulario() {
    if (this.modoEdicion) {
      this.userManagerService.UpdateUser(this.usuarioForm.id!, this.usuarioForm).subscribe({
        next: () => {
          this.cargarUsuarios();
          this.mostrarFormulario = false;
        },
        error: (err) => console.error('Error actualizando usuario', err),
      });
    } else {
      this.userManagerService.CreateUser(this.usuarioForm).subscribe({
        next: () => {
          this.cargarUsuarios();
          this.mostrarFormulario = false;
        },
        error: (err) => console.error('Error creando usuario', err),
      });
    }
  }

  eliminarUsuario(user: UsuarioGet) {
    if (confirm(`¿Seguro que quieres eliminar a ${user.usuarioNombre}?`)) {
      this.userManagerService.DeleteUser(user.id!).subscribe({
        next: () => this.cargarUsuarios(),
        error: (err) => console.error('Error eliminando usuario', err),
      });
    }
  }
}
