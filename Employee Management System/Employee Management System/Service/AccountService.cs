using Employee_Management_System.DTO;
using Employee_Management_System.Models;
using Microsoft.AspNetCore.Identity;
using System.Net.Mail;

namespace Employee_Management_System.Service
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<Employee> _userManager;
        private readonly JwtTokenService _jwtTokenService;

        public AccountService(UserManager<Employee> userManager, JwtTokenService jwtTokenService)
        {
            _userManager = userManager;
            _jwtTokenService = jwtTokenService;
        }

        public async Task<AuthResponseDTO> RegisterAsync(RegisterDTO registerDTO)
        {
            if (!IsValidEmail(registerDTO.Email))
            {
                throw new Exception("Invalid email format.");
            }

            if (await _userManager.FindByEmailAsync(registerDTO.Email) != null)
            {
                throw new Exception("Email is already registered.");
            }
            var employee = new Employee
            {
                UserName = registerDTO.Email,
                Name = registerDTO.Name,
                Email = registerDTO.Email,
                Department = registerDTO.Department,
                PhoneNumber = registerDTO.PhoneNumber,
                DateOfJoining = registerDTO.DateOfJoining,
            };

            var result = await _userManager.CreateAsync(employee, registerDTO.Password);
            if (!result.Succeeded)
            {
                throw new Exception("User registration failed: " + string.Join(", ", result.Errors.Select(e => e.Description)));
            }

            // Assign the default role "User"
            if (!await _userManager.IsInRoleAsync(employee, "User"))
            {
                await _userManager.AddToRoleAsync(employee, "User");
            }
            var employeeRole = await _userManager.GetRolesAsync(employee);
            var token = await _jwtTokenService.GenerateJwtToken(employee);

            return new AuthResponseDTO
            {
                Message = "User registered successfully.",
                userId = employee.Id,
                Role = employeeRole.ToArray()
            };
        }


        public async Task<AuthResponseDTO> LoginAsync(LoginDTO loginDTO)
        {
            if (!IsValidEmail(loginDTO.Email))
            {
                throw new Exception("Invalid email format.");
            }

            var employee = await _userManager.FindByEmailAsync(loginDTO.Email);
            if (employee == null)
            {
                throw new Exception("User not found.");
            }

            // Validate password
            var result = await _userManager.CheckPasswordAsync(employee, loginDTO.Password);
            if (!result)
            {
                throw new Exception("Password Is Inccorect.");
            }

            employee.LastLoginDate = DateOnly.FromDateTime(DateTime.Now);
            await _userManager.UpdateAsync(employee);

            var employeeRole = await _userManager.GetRolesAsync(employee);
            var token = await _jwtTokenService.GenerateJwtToken(employee);
            return new AuthResponseDTO
            {
                Message = "Login successfully.",
                userId = employee.Id,
                Token = token,
                Role = employeeRole.ToArray(),
            };
        }


        // Helper method to validate email format
        private bool IsValidEmail(string email)
        {
            try
            {
                var mail = new MailAddress(email);
                return mail.Address == email;
            }
            catch
            {
                return false;
            }
        }

        public async Task EnsureDefaultAdminAsync()
        {
            var adminEmail = "admin@admin.com";
            var adminPassword = "Admin@12345";
            var adminName = "Ahmed Helal";
            var adminPhone = "01007458070";
            var adminDepartment = "IT";
            var adminDateOfJoining = DateOnly.FromDateTime(DateTime.Now);


            var adminUser = await _userManager.FindByEmailAsync(adminEmail);
            if (adminUser == null)
            {
                var employee = new Employee
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    Name = adminName,
                    PhoneNumber = adminPhone,
                    Department = adminDepartment,
                    DateOfJoining = adminDateOfJoining
                };

                var result = await _userManager.CreateAsync(employee, adminPassword);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(employee, "Admin");
                }
            }
        }


    }
}