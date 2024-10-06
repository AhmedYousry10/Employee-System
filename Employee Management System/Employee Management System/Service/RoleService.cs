using Employee_Management_System.IService;
using Employee_Management_System.Models;
using Microsoft.AspNetCore.Identity;

namespace Employee_Management_System.Service
{
    public class RoleService : IRoleService
    {
        private readonly UserManager<Employee> _userManager;
        private readonly RoleManager<Role> _roleManager;
        public RoleService(RoleManager<Role> roleManager, UserManager<Employee> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        // Get all roles
        public async Task<List<Role>> GetRoleAsync()
        {
            var roleList = _roleManager.Roles.ToList();
            return roleList;
        }

        // Get roles for a Employees by their email
        public async Task<List<string>> GetEmployeeRoleAsync(string emailId)
        {
            var employee = await _userManager.FindByEmailAsync(emailId);
            if (employee == null)
            {
                throw new Exception("Employee not found.");
            }

            var employeeRoles = await _userManager.GetRolesAsync(employee);

            return employeeRoles.ToList();
        }

        // Add new roles if they don't already exist
        public async Task<List<string>> AddRolesAsync(string[] roles)
        {
            var roleList = new List<string>();
            foreach (var role in roles)
            {
                if (!await _roleManager.RoleExistsAsync(role))
                {
                    var result = await _roleManager.CreateAsync(new Role { Name = role });
                    if (result.Succeeded)
                    {
                        roleList.Add(role);
                    }
                    else
                    {
                        throw new Exception($"Failed to create role: {role}");
                    }
                }
            }
            return roleList;
        }

        // Assign roles to a Employee by their email
        public async Task<bool> AddEmployeeRoleAsync(string employeeEmail, string[] roles)
        {
            var employee = await _userManager.FindByEmailAsync(employeeEmail);

            if (employee == null)
            {
                throw new Exception("Employee not found.");

            }

            var validRoles = await ExistsRolesAsync(roles);
            if (validRoles.Count != roles.Length)
            {
                throw new Exception("Some roles do not exist.");
            }

            var result = await _userManager.AddToRolesAsync(employee, validRoles);
            return result.Succeeded;
        }

        // Helper method to check if roles exist
        private async Task<List<string>> ExistsRolesAsync(string[] roles)
        {
            var roleList = new List<string>();
            foreach (var role in roles)
            {
                var roleExist = await _roleManager.RoleExistsAsync(role);
                if (roleExist)
                {
                    roleList.Add(role);
                }
            }
            return roleList;
        }

        // Remove roles from a Employees by their email
        public async Task<bool> RemoveEmployeeRoleAsync(string employeeEmail, string[] roles)
        {
            var employee = await _userManager.FindByEmailAsync(employeeEmail);
            if (employee == null)
            {
                throw new Exception("Employee not found.");
            }

            var result = await _userManager.RemoveFromRolesAsync(employee, roles);
            return result.Succeeded;
        }

        // Ensure default roles exist
        public async Task EnsureDefaultRolesAsync()
        {
            var roles = new[] { "Admin", "User" };

            foreach (var role in roles)
            {
                if (!await _roleManager.RoleExistsAsync(role))
                {
                    await _roleManager.CreateAsync(new Role { Name = role });
                }
            }
        }
    }
}