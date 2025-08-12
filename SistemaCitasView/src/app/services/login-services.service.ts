import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment.development';
import { Usuario } from '../Models/Usuario';

@Injectable({
  providedIn: 'root'
})
export class LoginServicesService {
  url = 'Usuario';
  constructor(private http : HttpClient) { }

  public createUser(data : Usuario){
    return this.http.post<any>(`${environment.apiUrl}/${this.url}`, data);
  }

    public loginAuth(data : any) {
      return this.http.post<any>(`${environment.apiUrl}/InicioSesion/login`, data);
    }
}
