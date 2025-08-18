import { Component, Input, OnInit } from "@angular/core";
import { AppointmentsServices } from "../../services/appointments-services";
import { Turnos } from "../../Models/Turnos";
import { Citas } from "../../Models/Citas";
import { AuthService } from "../../services/auth-service.service";
import { forkJoin } from 'rxjs';

@Component({
  selector: 'AppointmentComponent',
  template: `
    <div class="p-6 space-y-8">

      <!-- Tabla 1: Turnos del usuario -->
      <div>
        <div class="flex justify-between items-center mb-4">
          <h2 class="text-xl font-semibold">Citas a las que estas suscrito</h2>
        </div>

        <table class="table-auto w-full bg-white shadow rounded overflow-hidden">
          <thead class="bg-gray-800 text-white">
            <tr>
              <th class="px-4 py-2 text-left">ID</th>
              <th class="px-4 py-2 text-left">Fecha</th>
              <th class="px-4 py-2 text-left">Id Horario</th>
              <th class="px-4 py-2 text-left">Estaciones</th>
              <th class="px-4 py-2 text-left">Tiempo Cita</th>
              <th class="px-4 py-2 text-center">Acciones</th>
            </tr>
          </thead>
          <tbody>
            <tr *ngFor="let t of turnosDelUsuario" class="border-b">
              <td class="px-4 py-2">{{ t.id }}</td>
              <td class="px-4 py-2">{{ t.fecha | date:'yyyy-MM-dd' }}</td>
              <td class="px-4 py-2">{{ t.idHorario }}</td>
              <td class="px-4 py-2">{{ t.estacionesCantidad }}</td>
              <td class="px-4 py-2">{{ t.tiempoCita }}</td>
              <td class="px-4 py-2">
                <div class="flex gap-2 justify-center">
                  <button
                    type="button"
                    class="bg-red-500 hover:bg-red-600 text-white px-3 py-1 rounded"
                    (click)="DeleteAppointment(t.id)"
                  >
                    Eliminar
                  </button>
                </div>
              </td>
            </tr>
            <tr *ngIf="!turnosDelUsuario?.length">
              <td class="px-4 py-4 text-center text-gray-500" colspan="7">
                No tienes citas registradas.
              </td>
            </tr>
          </tbody>
        </table>
      </div>

      <!-- Tabla 2: Turnos disponibles -->
      <div>
        <div class="flex justify-between items-center mb-4">
          <h2 class="text-xl font-semibold">turnos disponibles</h2>
        </div>

        <table class="table-auto w-full bg-white shadow rounded overflow-hidden">
          <thead class="bg-gray-800 text-white">
            <tr>
              <th class="px-4 py-2 text-left">ID</th>
              <th class="px-4 py-2 text-left">Fecha</th>
              <th class="px-4 py-2 text-left">Id Horario</th>
              <th class="px-4 py-2 text-left">Estaciones</th>
              <th class="px-4 py-2 text-left">Tiempo Cita</th>
              <th class="px-4 py-2 text-center">Acciones</th>
            </tr>
          </thead>
          <tbody>
            <tr *ngFor="let t of turnosDisponibles" class="border-b">
              <td class="px-4 py-2">{{ t.id }}</td>
              <td class="px-4 py-2">{{ t.fecha | date:'yyyy-MM-dd' }}</td>
              <td class="px-4 py-2">{{ t.idHorario }}</td>
              <td class="px-4 py-2">{{ t.estacionesCantidad }}</td>
              <td class="px-4 py-2">{{ t.tiempoCita }}</td>
              <td class="px-4 py-2">
                <div class="flex gap-2 justify-center">
                  <button
                    type="button"
                    class="bg-green-600 hover:bg-green-700 text-white px-3 py-1 rounded"
                    (click)="AddAppointment(t.id, this.userId)"
                  >
                    Agendar
                  </button>
                </div>
              </td>
            </tr>
            <tr *ngIf="!turnosDisponibles?.length">
              <td class="px-4 py-4 text-center text-gray-500" colspan="7">
                No hay turnos disponibles por el momento.
              </td>
            </tr>
          </tbody>
        </table>
      </div>

    </div>
  `,
})
export class AppointmentComponent implements OnInit {
  turnosDisponibles: Turnos[] = [];
  turnosDelUsuario: Turnos[] = [];
  userId : number = 0;
  roleInAppointment = Input()

  constructor(private api: AppointmentsServices, private authService :AuthService ) {}
    

ngOnInit(): void {
  forkJoin({
    disponibles: this.api.getAllAvailableShifts(),
    usuario: this.api.getShiftsOfUser()
  }).subscribe(({ disponibles, usuario }) => {
    this.turnosDisponibles = disponibles;
    this.turnosDelUsuario = usuario;

    const idsAEliminar = new Set(this.turnosDelUsuario.map(obj => obj.id));
    this.turnosDisponibles = this.turnosDisponibles.filter(obj => !idsAEliminar.has(obj.id));
 
    console.log("Disponibles filtrados:", this.turnosDisponibles);
  });
 
  const token = this.authService.getToken();
  if (token) {
    const decoded = this.authService.decodeToken();
    if (decoded && decoded['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name']) {
      this.userId = decoded['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier'];
    }
  }
}

DeleteAppointment(id: number) {
  this.api.deletebyId(id).subscribe({
    next: (res) => {
      console.log('Cita eliminada', res);
      this.api.getShiftsOfUser().subscribe(data => this.turnosDelUsuario = data);
    },
    error: (err) => console.error('Error eliminando cita', err)
  });
}

AddAppointment(turnoId: number, usuarioId: number){
  this.api.postAppointment(usuarioId, turnoId).subscribe({
    next: (res) => {
      console.log('Cita agendada', res);
      this.api.getShiftsOfUser().subscribe(data => this.turnosDelUsuario = data);
      this.api.getAllAvailableShifts().subscribe(data => this.turnosDisponibles = data);
    },
    error: (err) => console.error('Error agendando cita', err)
  });
}

}


