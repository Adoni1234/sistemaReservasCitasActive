import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoginComponent } from './Components/login.component';
import { RegisterComponent } from './Components/register.component';
import { UserManagerComponent } from './Components/BackOffice/userManager.component';
import { BusinessDateComponent } from './Components/BackOffice/BusinessDate.component';
import { ShiftsComponent } from './Components/BackOffice/Shifts.component';
import { SchedulesComponent } from './Components/BackOffice/Schedules.component';

const routes: Routes = [
  {
    path: '', // Ruta raíz para login
    component: LoginComponent
  },
  {
    path: 'registro', 
    component: RegisterComponent
  },
  {
    path: 'userManager', 
    component: UserManagerComponent
  },
  {
    path: 'BusinessDate', 
    component: BusinessDateComponent
  },
  {
    path: 'Shifts', 
    component: ShiftsComponent
  },
  {
    path: 'Schedules', 
    component: SchedulesComponent
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
