import { Component, OnInit } from "@angular/core";
import { AdminServiceService } from "../../services/backOffice/admin-service.service";
import { fechaHabiles } from "../../Models/FechaHabiles";
import { formatDate } from "@angular/common";

@Component({
  selector: "BusinessDateComponent",
  template: `
    <div class="p-4">
      <h2 class="text-xl font-bold mb-4">Gestión de Fechas Hábiles</h2>

      <!-- Mensaje del sistema -->
      <div
        *ngIf="message"
        class="mb-4 p-2 rounded text-white"
        [ngClass]="{
          'bg-green-500': messageType === 'success',
          'bg-red-500': messageType === 'error'
        }"
      >
        {{ message }}
      </div>

      <!-- Calendario -->
      <input
        type="date"
        class="border p-2 rounded mb-4"
        [min]="today"
        (change)="onDateSelect($event)"
      />

      <button
        class="bg-blue-500 hover:bg-blue-600 text-white px-4 py-2 rounded"
        (click)="createBusinessDate()"
        [disabled]="!selectedDate"
      >
        Agregar Fecha
      </button>

      <!-- Tabla -->
      <table class="table-auto w-full mt-4 border">
        <thead>
          <tr class="bg-gray-200">
            <th class="px-4 py-2">ID</th>
            <th class="px-4 py-2">Fecha</th>
          </tr>
        </thead>
        <tbody>
          <tr *ngFor="let fecha of businessDates">
            <td class="border px-4 py-2">{{ fecha.id }}</td>
            <td class="border px-4 py-2">{{ formatFecha(fecha.fecha) }}</td>
          </tr>
        </tbody>
      </table>
    </div>
  `,
})
export class BusinessDateComponent implements OnInit {
  businessDates: fechaHabiles[] = [];
  selectedDate: string | null = null;
  today: string = "";
  existingDatesSet: Set<string> = new Set();

  // Mensajes del sistema
  message: string | null = null;
  messageType: "success" | "error" = "success";

  constructor(private adminService: AdminServiceService) {
    const now = new Date();
    this.today = now.toISOString().split("T")[0];
  }

  ngOnInit() {
    this.loadBusinessDates();
  }

  // Cargar fechas desde el backend
  loadBusinessDates() {
    this.adminService.getBusinessDates().subscribe((data) => {
      this.businessDates = data;
      this.existingDatesSet = new Set(
        data.map((f) => new Date(f.fecha).toISOString().split("T")[0])
      );
    });
  }

  // Selección de fecha
  onDateSelect(event: any) {
    const dateValue = event.target.value;

    if (this.existingDatesSet.has(dateValue)) {
      this.showMessage("Esta fecha ya está registrada.", "error");
      this.selectedDate = null;
      event.target.value = "";
      return;
    }
    this.selectedDate = dateValue;
  }

  // Crear nueva fecha hábil
  createBusinessDate() {
    if (!this.selectedDate) return;

    const newDate = { Fecha: new Date(this.selectedDate) };

    this.adminService.CreateBusinessDates(newDate).subscribe({
      next: () => {
        // Agregar nueva fecha a la lista sin recargar
        this.businessDates.push({
        id: this.businessDates.length + 1,
        fecha: new Date(this.selectedDate!), // convertir a Date
        });

        this.existingDatesSet.add(this.selectedDate!);

        this.showMessage("Fecha registrada correctamente.", "success");
        this.selectedDate = null;
      },
      error: () => {
        this.showMessage("Error al registrar la fecha.", "error");
      },
    });
  }

  // Formatear fecha
  formatFecha(fecha: Date | string) {
    return formatDate(fecha, "dd/MM/yyyy", "en-US");
  }

  // Mostrar mensajes del sistema
  showMessage(msg: string, type: "success" | "error") {
    this.message = msg;
    this.messageType = type;
    setTimeout(() => (this.message = null), 3000);
  }
}
