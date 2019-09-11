import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { DashboardComponent } from './admin/dashboard/dashboard.component';
import { EmployeedetailsComponent } from './admin/employeedetails/employeedetails.component';
import { AddemployeeComponent } from './addemployee/addemployee.component';

const routes: Routes = [

  {path:"dashboard" , component:DashboardComponent},
  {path:'employeedetails' , component:EmployeedetailsComponent},
  {path:'addemployee' , component:AddemployeeComponent}


];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
