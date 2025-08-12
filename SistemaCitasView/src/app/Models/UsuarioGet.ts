export interface UsuarioGet {
  id?: number;  // parece que hay id
  usuarioNombre: string;
  password: string;
  email: string;
  rol: number;
  citas?: any;
}
