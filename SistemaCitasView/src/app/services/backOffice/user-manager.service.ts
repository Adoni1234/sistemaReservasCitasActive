import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment.development';
import { Usuario } from '../../Models/Usuario';
import { AuthService } from '../auth-service.service';
import { UsuarioGet } from '../../Models/UsuarioGet';

@Injectable({
  providedIn: 'root'
})
export class UserManagerService {
  url = "Usuario";

  constructor(private http: HttpClient, private authService: AuthService) {}

  private getHeaders(): HttpHeaders {
    const token = this.authService.getToken();
    return new HttpHeaders({
      'Authorization': `Bearer ${token}`,
      'Content-Type': 'application/json'
    });
  }

  public CreateUser(data: any) {
     return this.http.post<any>(`${environment.apiUrl}/${this.url}`,data,{ headers: this.getHeaders() }
    );
  }

  public GetAllUser() {
    return this.http.get<UsuarioGet[]>(`${environment.apiUrl}/${this.url}`,{ headers: this.getHeaders() });
  }

  public GetUserById(id: number) {
     return this.http.get<UsuarioGet>(`${environment.apiUrl}/${this.url}/${id}`,{ headers: this.getHeaders() }
    );
  }
  public UpdateUser(id: number, data: UsuarioGet) {
    return this.http.put<any>(
      `${environment.apiUrl}/${this.url}/${id}`,
      data,
      { headers: this.getHeaders() }
    );
  }

  public DeleteUser(id: number) {
      return this.http.delete<any>(`${environment.apiUrl}/${this.url}/${id}`,{ headers: this.getHeaders() }
    );
  }
}
