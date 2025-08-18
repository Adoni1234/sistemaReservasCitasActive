import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { AuthService } from '../auth-service.service';
import { environment } from '../../../environments/environment.development';
import { Scheduler } from 'timers/promises';
import { Horaios } from '../../Models/Horarios';
import { Turnos } from '../../Models/Turnos';
import { fechaHabiles } from '../../Models/FechaHabiles';

@Injectable({
  providedIn: 'root'
})
export class AdminServiceService {

  url = "Configuracion"
  constructor(private http: HttpClient, private authService: AuthService) {}

  private getHeaders(): HttpHeaders {
    const token = this.authService.getToken();
    return new HttpHeaders({
      'Authorization': `Bearer ${token}`,
      'Content-Type': 'application/json'
    });
  }

    public CreateBusinessDates(fecha: any) {
       return this.http.post<any>(`${environment.apiUrl}/${this.url}/fechas-habilitadas`,fecha,{ headers: this.getHeaders() }
      );
    }

    public CreateShifts(data: Turnos) {
       return this.http.post<any>(`${environment.apiUrl}/${this.url}/turnos`,data,{ headers: this.getHeaders() }
      );
    }

    public CreateSchedules(data: Horaios) {
       return this.http.post<any>(`${environment.apiUrl}/${this.url}/horarios`, data, { headers: this.getHeaders() });
      
    }

    public getBusinessDatesById(id: number) {
       return this.http.get<fechaHabiles>(`${environment.apiUrl}/${this.url}/fechas-habilitadas/${id}`,{ headers: this.getHeaders() }
      );
    }

    public getShiftsbyid(id: number) {
       return this.http.get<any>(`${environment.apiUrl}/${this.url}/turnos/${id}`,{ headers: this.getHeaders() }
      );
    }

    public getSchedulesbyid(id: number) {
       return this.http.get<any>(`${environment.apiUrl}/${this.url}/horarios/${id}`,{ headers: this.getHeaders() }
      );
    }
//

public getBusinessDates() {
  return this.http.get<fechaHabiles[]>(`${environment.apiUrl}/${this.url}/fechas-habilitadas`, { headers: this.getHeaders() });
}


    public getShifts() {
       return this.http.get<Turnos[]>(`${environment.apiUrl}/${this.url}/turnos`,{ headers: this.getHeaders() }
      );
    }

    public getSchedules() {
       return this.http.get<Horaios[]>(`${environment.apiUrl}/${this.url}/horarios`,{ headers: this.getHeaders() }
      );
    }

}
