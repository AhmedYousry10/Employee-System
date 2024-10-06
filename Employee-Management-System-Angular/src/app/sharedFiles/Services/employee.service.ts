import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Employee } from '../Models/employee';

@Injectable({
  providedIn: 'root'
})
export class EmployeeService {

  private baseUrl = 'https://localhost:7176/api/Employee';

  constructor(private http: HttpClient) { }

  getEmployees(pageNumber: number = 1, pageSize: number = 10): Observable<any> {
    return this.http.get<Employee[]>(`${this.baseUrl}?pageNumber=${pageNumber}&pageSize=${pageSize}`);
  }

  getEmployeeByEmail(email: string): Observable<any> {
    return this.http.get(`${this.baseUrl}/${email}`);
  }

  createEmployee(employee: any): Observable<any> {
    return this.http.post(this.baseUrl, employee);
  }

  updateEmployee(employee: any): Observable<any> {
    return this.http.put(`${this.baseUrl}/${employee.email}`, employee);
  }

  deleteEmployee(email: string): Observable<any> {
    return this.http.delete(`${this.baseUrl}/${email}`);
  }

}
