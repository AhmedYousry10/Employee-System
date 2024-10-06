import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { Employee } from '../../sharedFiles/Models/employee';
import { EmployeeService } from '../../sharedFiles/Services/employee.service';
import { ConfirmationService, MessageService } from 'primeng/api';
import { ButtonModule } from 'primeng/button';
import { DialogModule } from 'primeng/dialog';
import { InputTextModule } from 'primeng/inputtext';
import { ToastModule } from 'primeng/toast';
import { ConfirmDialogModule } from 'primeng/confirmdialog';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-add-employee',
  standalone: true,
  imports: [
    ButtonModule,
    DialogModule,
    InputTextModule,
    ToastModule,
    ConfirmDialogModule,
    CommonModule,
    RouterModule,
    FormsModule,
  ],
  providers: [ConfirmationService, MessageService],
  templateUrl: './add-employee.component.html',
  styleUrls: ['./add-employee.component.css'], // Corrected plural
})
export class AddEmployeeComponent implements OnInit {


  // employee: Employee = {
  //   name: '',
  //   email: '',
  //   password: '',
  //   phoneNumber: '',
  //   department: '',
  //   dateOfJoining: new Date(),
  //   isActive: 'true'
  // };

  employee: Employee = {} as Employee;

  displayAddModal: boolean = false;
  departments: string[] = ['Front-End', 'Back-End', 'HR'];
  isActiveOptions: { label: string; value: boolean }[] = [
    { label: 'Yes', value: true },
    { label: 'No', value: false },
  ];

  @Output() employeeAdded: EventEmitter<void> = new EventEmitter<void>();

  constructor(
    private employeeService: EmployeeService,
    private messageService: MessageService
  ) {}

  ngOnInit(): void {
    // No need for getEmployee
  }

  showAddModal() {
    this.displayAddModal = true;
  }

  hideAddModal() {
    this.displayAddModal = false;
  }

  setEmployeeAcriveStatus(val: 'true' | 'false') {
    this.employee.isActive = val === 'true';
  }

  addEmployee() {
    this.employeeService.createEmployee(this.employee).subscribe(() => {
      this.messageService.add({
        severity: 'success',
        summary: 'Success',
        detail: 'Employee added successfully',
      });
      this.resetEmployee();
      this.employeeAdded.emit();
      this.displayAddModal = false;
    });
  }

  resetEmployee() {
    this.employee = {
      name: '',
      email: '',
      password: '',
      phoneNumber: '',
      department: '',
      dateOfJoining: new Date(),
      isActive: true,
    };
  }
}
