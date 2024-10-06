import { Routes } from '@angular/router';
import { NotFoundComponent } from './not-found/not-found.component';
import { NavbarComponent } from './navbar/navbar.component';
import { EmployeeListComponent } from './employee/employee-list/employee-list.component';
import { DashboardComponent } from './dashboard/dashboard.component';
import { AddEmployeeComponent } from './employee/add-employee/add-employee.component';

export const routes: Routes = [
  { path: 'navbar', component: NavbarComponent },
  { path: 'employee/employee-List', component: EmployeeListComponent },
  { path: 'dashboard', component: DashboardComponent },

  { path: '', redirectTo: 'dashboard', pathMatch: 'full' },
  { path: '**', component: NotFoundComponent }
];
