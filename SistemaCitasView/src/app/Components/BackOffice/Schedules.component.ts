import { Component, OnInit } from "@angular/core";
import { AdminServiceService } from "../../services/backOffice/admin-service.service";
import { Horaios } from "../../Models/Horarios";

@Component({
  selector: "SchedulesComponent",
  template: `
    <div class="p-4">
      <h2 class="text-xl font-bold mb-4 text-center">Gestión de Horarios</h2>

      <!-- Mensaje del sistema -->
      <div
        *ngIf="message"
        class="mb-4 p-2 rounded text-white text-center"
        [ngClass]="{
          'bg-green-500': messageType === 'success',
          'bg-red-500': messageType === 'error'
        }"
      >
        {{ message }}
      </div>

      <!-- Botón para mostrar formulario -->
      <div class="mb-4 text-center">
        <button
          class="bg-blue-500 hover:bg-blue-600 text-white px-4 py-2 rounded"
          (click)="showForm = !showForm"
        >
          {{ showForm ? 'Ocultar Formulario' : 'Crear Horario' }}
        </button>
      </div>

      <!-- Formulario para nuevo horario -->
      <div *ngIf="showForm" class="mb-4 flex flex-col gap-2">
        <input
          type="text"
          class="border p-2 rounded"
          placeholder="Nombre del turno"
          [(ngModel)]="nombreTurno"
        />
        <input
          type="time"
          class="border p-2 rounded"
          placeholder="Inicio"
          [(ngModel)]="inicio"
        />
        <input
          type="time"
          class="border p-2 rounded"
          placeholder="Fin"
          [(ngModel)]="fin"
        />
        <button
          class="bg-green-500 hover:bg-green-600 text-white px-4 py-2 rounded"
          (click)="createSchedule()"
          [disabled]="!nombreTurno || !inicio || !fin"
        >
          Agregar Horario
        </button>
      </div>

      <!-- Tabla de horarios -->
      <table class="table-auto w-full border text-center">
        <thead>
          <tr class="bg-gray-200">
            <th class="px-4 py-2">ID</th>
            <th class="px-4 py-2">Nombre Turno</th>
            <th class="px-4 py-2">Inicio</th>
            <th class="px-4 py-2">Fin</th>
          </tr>
        </thead>
        <tbody>
          <tr *ngFor="let horario of schedules">
            <td class="border px-4 py-2">{{ horario.id }}</td>
            <td class="border px-4 py-2">{{ horario.nombreTurno }}</td>
            <td class="border px-4 py-2">{{ horario.inicio }}</td>
            <td class="border px-4 py-2">{{ horario.fin }}</td>
          </tr>
        </tbody>
      </table>
    </div>
  `,
})
export class SchedulesComponent implements OnInit {
  schedules: Horaios[] = [];
  showForm: boolean = false;

  nombreTurno: string = "";
  inicio: string = "";
  fin: string = "";

  message: string | null = null;
  messageType: "success" | "error" = "success";

  constructor(private adminService: AdminServiceService) {}

  ngOnInit() {
    this.loadSchedules();
  }

  loadSchedules() {
    this.adminService.getSchedules().subscribe((data) => {
      this.schedules = data;
    });
  }

  createSchedule() {
    if (!this.nombreTurno || !this.inicio || !this.fin) return;

    const newSchedule: Horaios = {
      id: this.schedules.length + 1,
      nombreTurno: this.nombreTurno,
      inicio: this.inicio,
      fin: this.fin,
    };

    this.adminService.CreateSchedules(newSchedule).subscribe({
      next: () => {
        this.schedules.push(newSchedule);
        this.showMessage("Horario registrado correctamente.", "success");

        // Limpiar campos
        this.nombreTurno = "";
        this.inicio = "";
        this.fin = "";
      },
      error: () => {
        this.showMessage("Error al registrar el horario.", "error");
      },
    });
  }

  showMessage(msg: string, type: "success" | "error") {
    this.message = msg;
    this.messageType = type;
    setTimeout(() => (this.message = null), 3000);
  }
}
