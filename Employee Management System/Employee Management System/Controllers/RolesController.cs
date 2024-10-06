using Employee_Management_System.IService;
using Employee_Management_System.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace Employee_Management_System.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly IRoleService _roleService;
        public RolesController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpGet("getRoles")]
        [SwaggerOperation(summary: "Get all roles", description: "Get all roles")]
        [SwaggerResponse(statusCode: 200, description: "Success")]
        public async Task<ActionResult> GetRoles()
        {
            var rolesList = await _roleService.GetRoleAsync();
            return Ok(rolesList);
        }
        
        [HttpGet("getEmployeeRoles")]
        [SwaggerOperation(summary: "Get roles for a Employee by their email", description: "Get roles for a Employee by their email")]
        [SwaggerResponse(statusCode: 200, description: "Success")]
        public async Task<ActionResult> GetEmployeeRoles(string employeeEmail)
        {
            if (string.IsNullOrWhiteSpace(employeeEmail))
            {
                return BadRequest("Employee email is required.");
            }
            var employeeData = await _roleService.GetEmployeeRoleAsync(employeeEmail);
            return Ok(employeeData);
        }

        [HttpPost("addRoles")]
        [SwaggerOperation(summary: "Add new roles if they don't already exist", description: "Add new roles if they don't already exist")]
        [SwaggerResponse(statusCode: 200, description: "Success")]
        public async Task<ActionResult> AddRoles(string[] roles)
        {
            if (roles == null || roles.Length == 0)
            {
                return BadRequest("At least one role must be provided.");
            }

            var newRoles = await _roleService.AddRolesAsync(roles);
            if (newRoles.Count == 0)
            {
                return NoContent();
            }
            return Ok($"New role(s) added: {string.Join(", ", newRoles)}");
        }

        [HttpPost("addEmployeeRoles")]
        [SwaggerOperation(summary: "Add roles to a Employee", description: "Add roles to a Employee")]
        [SwaggerResponse(statusCode: 201, description: "Success")]
        public async Task<ActionResult> AddEmployeeRoles([FromBody] EmployeeRole employeeRole)
        {
            if (employeeRole == null || string.IsNullOrWhiteSpace(employeeRole.EmployeeEmail) || employeeRole.Roles == null)
            {
                return BadRequest("Employee email and roles must be provided.");
            }

            var result = await _roleService.AddEmployeeRoleAsync(employeeRole.EmployeeEmail, employeeRole.Roles);
            if (!result)
            {
                return BadRequest();
            }
            return StatusCode((int)HttpStatusCode.Created, result);

        }

        [HttpDelete("removeEmployeeRoles")]
        [SwaggerOperation(summary: "Remove roles from a Employee", description: "Remove roles from a Employee")]
        [SwaggerResponse(statusCode: 200, description: "Roles removed successfully.")]
        [SwaggerResponse(statusCode: 400, description: "Invalid input provided.")]
        [SwaggerResponse(statusCode: 404, description: "Employee or roles not found.")]
        public async Task<ActionResult> RemoveEmployeeRoles([FromBody] EmployeeRole employeeRole)
        {
            if (employeeRole == null ||
                string.IsNullOrWhiteSpace(employeeRole.EmployeeEmail) ||
                employeeRole.Roles == null ||
                !employeeRole.Roles.Any())
            {
                return BadRequest("Employee email and roles must be provided.");
            }

            try
            {
                var result = await _roleService.RemoveEmployeeRoleAsync(employeeRole.EmployeeEmail, employeeRole.Roles);
                if (!result)
                {
                    return NotFound("Employee or roles not found.");
                }

                return Ok(new { message = "Roles removed successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
