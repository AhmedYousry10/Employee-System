import { Component, OnInit, ViewChild } from '@angular/core';
import { Employee } from '../../sharedFiles/Models/employee';
import { PaginatorModule, PaginatorState } from 'primeng/paginator';
import { EmployeeService } from '../../sharedFiles/Services/employee.service';
import { CommonModule } from '@angular/common';
import { RouterLink, RouterModule } from '@angular/router';
import { AddEmployeeComponent } from "../add-employee/add-employee.component";
import Swal from 'sweetalert2';

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

  // deleteEmployee
  deleteEmployee(email: string): void {
    Swal.fire({
      title: 'Are you sure?',
      text: "You won't be able to revert this!",
      icon: 'warning',
      showCancelButton: true,
      confirmButtonColor: '#3085d6',
      cancelButtonColor: '#d33',
      confirmButtonText: 'Yes, delete it!'
    }).then((result) => {
      if (result.isConfirmed) {
        this.employeeService.deleteEmployee(email).subscribe(() => {
          this.getAllEmployees();

          Swal.fire(
            'Deleted!',
            'The employee has been deleted.',
            'success'
          );
        }, (error) => {
          console.error('Error deleting employee:', error);

          Swal.fire(
            'Error!',
            'Failed to delete the employee.',
            'error'
          );
        });
      }
    });
  }

}
