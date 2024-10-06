import { Component, OnInit } from '@angular/core';
import { EmployeeService } from '../sharedFiles/Services/employee.service';
import { MatCardModule } from '@angular/material/card';
import { MatGridListModule } from '@angular/material/grid-list';
import { MatListModule } from '@angular/material/list';
import { MatIconModule } from '@angular/material/icon';
import { MatBadgeModule } from '@angular/material/badge';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [MatCardModule,
    MatGridListModule,
    MatListModule,
    MatIconModule,
    MatBadgeModule],
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.css'
})
export class DashboardComponent implements OnInit {
  totalEmployees: number = 0;
  activeEmployees: number = 0;
  inactiveEmployees: number = 0;

  constructor(private employeeService: EmployeeService) { }

  ngOnInit(): void {
    this.loadDashboardData();
  }

  loadDashboardData(): void {
    this.employeeService.getEmployees().subscribe((data) => {
      this.totalEmployees = data.length;
      this.activeEmployees = data.filter((employee: any) => employee.isActive).length;
      this.inactiveEmployees = this.totalEmployees - this.activeEmployees;
    });
  }

}
