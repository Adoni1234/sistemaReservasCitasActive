import { NgModule } from '@angular/core';
import { BrowserModule, provideClientHydration } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { LoginComponent } from './Components/login.component';
import { RegisterComponent } from './Components/register.component';
import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule } from '@angular/forms';
import { UserManagerComponent } from './Components/BackOffice/userManager.component';
import { LayoutComponent } from './Components/Shared/layout ';
import { BusinessDateComponent } from './Components/BackOffice/BusinessDate.component';
import { ShiftsComponent } from './Components/BackOffice/Shifts.component';
import { SchedulesComponent } from './Components/BackOffice/Schedules.component';

@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    RegisterComponent,
    UserManagerComponent,
    LayoutComponent ,
    BusinessDateComponent,
    ShiftsComponent,
    SchedulesComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    CommonModule,
    HttpClientModule,
    FormsModule
  ],
  providers: [
    provideClientHydration()
  ],
  bootstrap: [AppComponent]  // Solo el root aquí
})

export class AppModule { }
