
import { Injectable } from '@angular/core';
import { AuthService } from './auth-service.service';
import { Turnos } from '../Models/Turnos';
import { environment } from '../../environments/environment.development';
import { Citas } from '../Models/Citas';
import { HttpClient, HttpHeaders } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class AppointmentsServices {
  url = "Cita";
  
   constructor(private http: HttpClient, private authService: AuthService) {}

  private getHeaders(): HttpHeaders {
    const token = this.authService.getToken();
    return new HttpHeaders({
      'Authorization': `Bearer ${token}`,
      'Content-Type': 'application/json'
    });
  }


  public getAllAvailableShifts() {
    return this.http.get<Turnos[]>(`${environment.apiUrl}/Configuracion/turnos`, { headers: this.getHeaders() }
    );
  }

  public getShiftsOfUser() {
    return this.http.get<Turnos[]>(`${environment.apiUrl}/Configuracion/turnos/usuarios/thisUser`, { headers: this.getHeaders() }
    );
  }

public postAppointment(usuarioId: number, turnoId: number) {
  return this.http.post<any>(
    `${environment.apiUrl}/${this.url}/reservar/${usuarioId}/${turnoId}`, {}, { headers: this.getHeaders() });
}

  public deletebyId(citaId: number) {
    return this.http.delete<any>(`${environment.apiUrl}/${this.url}/${citaId}`, { headers: this.getHeaders() }
    );
  }


}
