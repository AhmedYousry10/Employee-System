import { Component, OnInit, ViewChild } from '@angular/core';
import { Employee } from '../../sharedFiles/Models/employee';
import { PaginatorModule, PaginatorState } from 'primeng/paginator';
import { EmployeeService } from '../../sharedFiles/Services/employee.service';
import { CommonModule } from '@angular/common';
import { RouterLink, RouterModule } from '@angular/router';
import { AddEmployeeComponent } from "../add-employee/add-employee.component";

@Component({
  selector: 'app-employee-list',
  standalone: true,
  imports: [
    CommonModule,
    RouterLink,
    RouterModule,
    PaginatorModule,
    AddEmployeeComponent
],
  templateUrl: './employee-list.component.html',
  styleUrls: ['./employee-list.component.css']
})
export class EmployeeListComponent implements OnInit {
  title = 'Employee List';
  employeeList: Employee[] = [];
  filteredEmployeeList: Employee[] = [];
  paginatedEmployeeList: Employee[] = [];
  searchQuery: string = '';
  first : number = 0;
  rows : number = 5;

  constructor(private employeeService: EmployeeService) { }

  ngOnInit(): void {
    this.getAllEmployees();
  }

  getAllEmployees(): void {
    this.employeeService.getEmployees().subscribe((data) => {
      this.employeeList = data;
      this.filteredEmployeeList = this.employeeList;
      this.paginateEvents();
    });
  }

  paginateEvents() {
    console.log(this.first, this.rows);
    this.paginatedEmployeeList = this.filteredEmployeeList.slice(this.first, this.first + this.rows);
  }

  onSearch() {
    this.filteredEmployeeList = this.employeeList.filter((employee) => {
      return employee.name.toLowerCase().includes(this.searchQuery.toLowerCase());
    });
    this.paginateEvents();
  }

  onPageChange(event: PaginatorState) {
    this.first = event.first?.valueOf() || 0;
    this.rows = event.rows?.valueOf() || 5;
    this.paginateEvents();
  }


}
