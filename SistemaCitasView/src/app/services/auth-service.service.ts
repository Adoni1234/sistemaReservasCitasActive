// src/app/services/auth.service.ts
import { Injectable } from '@angular/core';
import { jwtDecode } from 'jwt-decode';


@Injectable({
  providedIn: 'root'
})
export class AuthService {

  private tokenKey = 'token'; // nombre usado en sessionStorage

  constructor() {}

  // Guardar token
  setToken(token: string): void {
    sessionStorage.setItem(this.tokenKey, token);
  }

  // Obtener token
  getToken(): string | null {
    return sessionStorage.getItem(this.tokenKey);
  }

  // Eliminar token
  removeToken(): void {
    sessionStorage.removeItem(this.tokenKey);
  }

  // Decodificar token
  decodeToken(): any | null {
    const token = this.getToken();
    if (!token) return null;

    try {
      return jwtDecode(token);
    } catch (e) {
      console.error('Error decodificando token', e);
      return null;
    }
  }

  // Saber si el token expiró
  isTokenExpired(): boolean {
    const decoded: any = this.decodeToken();
    if (!decoded || !decoded.exp) return true;

    const now = Math.floor(new Date().getTime() / 1000);
    return decoded.exp < now;
  }
}
