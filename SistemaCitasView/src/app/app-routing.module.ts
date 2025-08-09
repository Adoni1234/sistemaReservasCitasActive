import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoginComponent } from './Components/login.component';
import { RegisterComponent } from './Components/register.component';

const routes: Routes = [
  {
    path: '', // Ruta raíz para login
    component: LoginComponent
  },
  {
    path: 'registro', // Sin barra inicial
    component: RegisterComponent
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
