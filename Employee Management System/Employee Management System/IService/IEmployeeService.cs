using Employee_Management_System.DTO;

namespace Employee_Management_System.IService
{
    public interface IEmployeeService
    {
        Task<EmployeeDTO> GetEmployeeByEmailAsync(string employeeEmail);
        Task<List<EmployeeDTO>> GetAllEmployeesAsync(int pageNumber, int pageSize);
        Task<EmployeeDTO> CreateEmployeeAsync(EmployeeDTO employeeDto);
        Task<EmployeeDTO> UpdateEmployeeAsync(EmployeeDTO employeeDto);
        Task<string> DeleteEmployeeAsync(string employeeEmail);
    }
}
