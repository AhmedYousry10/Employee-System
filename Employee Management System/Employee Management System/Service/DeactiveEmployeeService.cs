using Employee_Management_System.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Employee_Management_System.Service
{
    public class DeactiveEmployeeService
    {
        private readonly UserManager<Employee> _userManager;

        public DeactiveEmployeeService(UserManager<Employee> userManager)
        {
            _userManager = userManager;
        }

        public async Task DeactivateEmployeesAsync()
        {
            var lastLoginDate = DateOnly.FromDateTime(DateTime.Now.AddDays(-90));

            var inactiveEmployees = await _userManager.Users
            .Where(e => e.LastLoginDate != null && e.LastLoginDate < lastLoginDate && e.IsActive)
            .ToListAsync();

            foreach (var employee in inactiveEmployees)
            {
                employee.IsActive = false;
                await _userManager.UpdateAsync(employee);
            }
        }
    }
}
