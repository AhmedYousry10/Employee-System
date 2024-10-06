using Employee_Management_System.DTO;
using Employee_Management_System.IService;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;

namespace Employee_Management_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;

        public EmployeeController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        // GET: api/employee
        [HttpGet]
        [SwaggerOperation(summary: "Get All Employee", description: "Get All Employee")]
        [SwaggerResponse(statusCode: 200, description: "Success")]
        public async Task<IActionResult> GetAllEmployees([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var employees = await _employeeService.GetAllEmployeesAsync(pageNumber, pageSize);
                return Ok(employees);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // GET: api/employee/{email}
        [HttpGet("{email}")]
        [SwaggerOperation(summary: "Get Employee by Email", description: "Get Employee by Email")]
        [SwaggerResponse(statusCode: 200, description: "Success")]
        [SwaggerResponse(statusCode: 404, description: "Employee not found")]
        public async Task<IActionResult> GetEmployeeByEmail(string email)
        {
            try
            {
                var employee = await _employeeService.GetEmployeeByEmailAsync(email);
                if (employee == null)
                {
                    return NotFound(new { message = "Employee not found" });
                }
                return Ok(employee);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // POST: api/employee
        [HttpPost]
        [SwaggerOperation(summary: "Create a new Employee", description: "Add a new Employee")]
        [SwaggerResponse(statusCode: 201, description: "Employee created")]
        [SwaggerResponse(statusCode: 400, description: "Invalid input")]
        [SwaggerResponse(statusCode: 500, description: "Internal server error")]
        public async Task<IActionResult> CreateEmployee([FromBody] EmployeeDTO employeeDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var createdEmployee = await _employeeService.CreateEmployeeAsync(employeeDto);
                return CreatedAtAction(nameof(GetEmployeeByEmail), new { email = createdEmployee.Email }, createdEmployee);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        // PUT: api/employee/{email}
        [HttpPut("{email}")]
        [SwaggerOperation(summary: "Update Employee by Email", description: "Update Employee by Email")]
        [SwaggerResponse(statusCode: 200, description: "Employee updated")]
        [SwaggerResponse(statusCode: 400, description: "Invalid input")]
        [SwaggerResponse(statusCode: 404, description: "Employee not found")]
        [SwaggerResponse(statusCode: 500, description: "Internal server error")]
        public async Task<IActionResult> UpdateEmployee(string email, [FromBody] EmployeeDTO employeeDto)
        {
            if (email != employeeDto.Email)
            {
                return BadRequest(new { message = "Employee email mismatch" });
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var updatedEmployee = await _employeeService.UpdateEmployeeAsync(employeeDto);
                if (updatedEmployee == null)
                {
                    return NotFound(new { message = "Employee not found" });
                }
                return Ok(updatedEmployee);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Employee not found.")
                {
                    return NotFound(new { message = ex.Message });
                }

                return StatusCode(500, new { message = ex.Message });
            }
        }

        // DELETE: api/employee/{email}
        [HttpDelete("{email}")]
        [SwaggerOperation(summary: "Delete Employee", description: "Delete Employee")]
        [SwaggerResponse(statusCode: 200, description: "Employee deleted")]
        [SwaggerResponse(statusCode: 400, description: "Invalid input")]
        [SwaggerResponse(statusCode: 500, description: "Internal server error")]
        public async Task<IActionResult> DeleteEmployee(string email)
        {
            try
            {
                var result = await _employeeService.DeleteEmployeeAsync(email);
                return Ok(new { message = result });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}
