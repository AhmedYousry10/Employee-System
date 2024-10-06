using Employee_Management_System.Models;

namespace Employee_Management_System.IService
{
    public interface IRoleService
    {
        Task<List<Role>> GetRoleAsync();
        Task<List<string>> GetEmployeeRoleAsync(string emailId);
        Task<List<string>> AddRolesAsync(string[] roles);
        Task<bool> AddEmployeeRoleAsync(string employeeEmail, string[] roles);
        Task<bool> RemoveEmployeeRoleAsync(string employeeEmail, string[] roles);
        Task EnsureDefaultRolesAsync();
    }
}
