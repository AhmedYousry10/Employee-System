using AutoMapper;
using Employee_Management_System.DTO;
using Employee_Management_System.IService;
using Employee_Management_System.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Employee_Management_System.Service
{
    public class EmployeeService : IEmployeeService
    {
        private readonly appContext _appContext;
        private readonly IMapper _mapper;
        private readonly UserManager<Employee> _userManager;

        public EmployeeService(appContext appContext, IMapper mapper, UserManager<Employee> userManager)
        {
            _appContext = appContext;
            _mapper = mapper;
            _userManager = userManager;
        }

        // Create new Employee
        public async Task<EmployeeDTO> CreateEmployeeAsync(EmployeeDTO employeeDto)
        {
            if (string.IsNullOrWhiteSpace(employeeDto.Email) || string.IsNullOrWhiteSpace(employeeDto.Password))
            {
                throw new ArgumentException("Email and Password are required.");
            }

            var existingEmployee = await _userManager.FindByEmailAsync(employeeDto.Email);
            if (existingEmployee != null)
            {
                throw new Exception("Employee with this email already exists.");
            }

            var employee = _mapper.Map<Employee>(employeeDto);
            employee.UserName = employeeDto.Email;
            employee.PhoneNumber = employeeDto.PhoneNumber;

            var result = await _userManager.CreateAsync(employee, employeeDto.Password);
            if (!result.Succeeded)
            {
                throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));
            }

            var EmployeeRole = await _userManager.AddToRoleAsync(employee, "User");
            if (!EmployeeRole.Succeeded)
            {
                throw new Exception(string.Join(", ", EmployeeRole.Errors.Select(e => e.Description)));
            }

            return _mapper.Map<EmployeeDTO>(employee);
        }

        // Delete Employee
        public async Task<string> DeleteEmployeeAsync(string employeeEmail)
        {
            if (string.IsNullOrWhiteSpace(employeeEmail))
            {
                throw new ArgumentException("User email cannot be null or empty.");
            }

            var employee = await _userManager.FindByEmailAsync(employeeEmail);
            if (employee == null)
            {
                throw new Exception("User not found.");
            }

            var result = await _userManager.DeleteAsync(employee);
            if (result.Succeeded)
            {
                return "Employee Deleted Successfully.";
            }
            else
            {
                throw new Exception("Failed to delete user.");
            }
        }

        // Get All Employees
        public async Task<List<EmployeeDTO>> GetAllEmployeesAsync(int pageNumber, int pageSize)
        {
            if (pageNumber < 1 || pageSize < 1)
            {
                throw new ArgumentException("Page number and page size must be greater than zero.");
            }

            var employees = await _userManager.Users
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            if (employees == null || employees.Count == 0)
            {
                throw new Exception("No employees found.");
            }

            return _mapper.Map<List<EmployeeDTO>>(employees);
        }

        // Get Employee by Email
        public async Task<EmployeeDTO> GetEmployeeByEmailAsync(string employeeEmail)
        {
            var employee = await _userManager.FindByEmailAsync(employeeEmail.ToString());
            if (employee == null || !employee.IsActive)
            {
                throw new Exception("Employee not found.");
            }

            return _mapper.Map<EmployeeDTO>(employee);
        }

        // Update Employee
        public async Task<EmployeeDTO> UpdateEmployeeAsync(EmployeeDTO employeeDto)
        {
            var employee = await _userManager.FindByEmailAsync(employeeDto.Email);
            if (employee == null)
            {
                throw new Exception("Employee not found.");
            }

            employee.Name = employeeDto.Name;
            employee.PhoneNumber = employeeDto.PhoneNumber;
            employee.Department = employeeDto.Department;
            employee.DateOfJoining = employeeDto.DateOfJoining;
            employee.IsActive = employeeDto.IsActive;

            if (!string.IsNullOrEmpty(employeeDto.Password))
            {
                employee.PasswordHash = _userManager.PasswordHasher.HashPassword(employee, employeeDto.Password);
            }

            var result = await _userManager.UpdateAsync(employee);

            if (!result.Succeeded)
            {
                throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));
            }

            return _mapper.Map<EmployeeDTO>(employee);
        }
    }
}
