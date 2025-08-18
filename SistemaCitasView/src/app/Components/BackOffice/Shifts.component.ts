import { Component, OnInit } from "@angular/core";
import { formatDate } from "@angular/common";
import { AdminServiceService } from "../../services/backOffice/admin-service.service";
import { Turnos } from "../../Models/Turnos";
import { Horaios } from "../../Models/Horarios";

@Component({
  selector: "ShiftsComponent",
  template: `
    <div class="p-4">
      <h2 class="text-xl font-bold mb-4 text-center">Gestión de Turnos</h2>

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
          {{ showForm ? 'Ocultar Formulario' : 'Crear Turno' }}
        </button>
      </div>

      <!-- Formulario para nuevo turno (solo visible cuando showForm es true) -->
      <div *ngIf="showForm" class="mb-4 flex flex-col gap-2">
        <input
          type="date"
          class="border p-2 rounded"
          [min]="today"
          [(ngModel)]="selectedDate"
        />
        <input
          type="number"
          class="border p-2 rounded"
          placeholder="Cantidad de estaciones"
          [(ngModel)]="estacionesCantidad"
        />
        <input
          type="number"
          class="border p-2 rounded"
          placeholder="Tiempo por cita (minutos)"
          [(ngModel)]="tiempoCita"
        />

        <select
        class="border p-2 rounded"
        placeholder="Seleccionar horario"
        [(ngModel)]="selectedHorarioId"
        >
        <option value="" disabled selected>-- Selecciona un horario --</option>
        <option *ngFor="let h of horarios" [value]="h.id">
          {{ h.inicio }} - {{ h.fin }} ({{ h.nombreTurno }})
        </option>
      </select>
        <button
          class="bg-green-500 hover:bg-green-600 text-white px-4 py-2 rounded"
          (click)="createShift()"
          [disabled]="!selectedDate || !estacionesCantidad || !tiempoCita"
        >
          Agregar Turno
        </button>
      </div>

      <!-- Tabla de turnos -->
      <table class="table-auto w-full border text-center">
        <thead>
          <tr class="bg-gray-200">
            <th class="px-4 py-2">ID</th>
            <th class="px-4 py-2">Fecha</th>
            <th class="px-4 py-2">Horario </th>
            <th class="px-4 py-2">Estaciones</th>
            <th class="px-4 py-2">Tiempo por cita</th>
          </tr>
        </thead>
        <tbody>
          <tr *ngFor="let turno of shifts">
            <td class="border px-4 py-2">{{ turno.id }}</td>
            <td class="border px-4 py-2">{{ formatFecha(turno.fecha) }}</td>
           <td class="border px-4 py-2">{{ getHoraByHorarioId(turno.idHorario) }}</td>

            <td class="border px-4 py-2">{{ turno.estacionesCantidad }}</td>
            <td class="border px-4 py-2">{{ turno.tiempoCita }} min</td>
          </tr>
        </tbody>
      </table>
    </div>
  `,
})
export class ShiftsComponent implements OnInit {
  shifts: Turnos[] = [];
  horarios: Horaios[] = []; // Solo en este componente
  selectedHorarioId: number = 0;
  selectedDate: string | null = null;
  estacionesCantidad: number | null = null;
  tiempoCita: number | null = null;
  today: string = "";
  showForm: boolean = false;
  message: string | null = null;
  messageType: "success" | "error" = "success";

  constructor(private adminService: AdminServiceService) {
    this.today = new Date().toISOString().split("T")[0];
  }

  ngOnInit() {
    this.loadSchedules();
    this.loadShifts();
  }

  loadSchedules() {
    this.adminService.getSchedules().subscribe((data) => {
      this.horarios = data;
    });
  }

  loadShifts() {
    this.adminService.getShifts().subscribe((data) => {
      this.shifts = data;
    });
  }

  getHoraByHorarioId(idHorario: number): string {
    const horario = this.horarios.find(h => h.id === idHorario);
    return horario ? horario.inicio  +  " - "+ horario.fin : `ID ${idHorario}`;
  }

  createShift() {
    if (!this.selectedDate || !this.estacionesCantidad || !this.tiempoCita) return;

    const newShift: Turnos = {
      id: this.shifts.length + 1,
      fecha: new Date(this.selectedDate),
      idHorario: this.selectedHorarioId,
      estacionesCantidad: this.estacionesCantidad,
      tiempoCita: this.tiempoCita,
    };

    this.adminService.CreateShifts(newShift).subscribe({
      next: () => {
        this.shifts.push(newShift);
        this.showMessage("Turno registrado correctamente.", "success");
        this.selectedDate = null;
        this.estacionesCantidad = null;
        this.tiempoCita = null;
      },
      error: () => {
        this.showMessage("Error al registrar el turno.", "error");
      },
    });
  }

  formatFecha(fecha: Date | string) {
    return formatDate(fecha, "dd/MM/yyyy", "en-US");
  }

  showMessage(msg: string, type: "success" | "error") {
    this.message = msg;
    this.messageType = type;
    setTimeout(() => (this.message = null), 3000);
  }
}
