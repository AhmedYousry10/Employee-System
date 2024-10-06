using System.ComponentModel.DataAnnotations;

namespace Employee_Management_System.DTO
{
    public class EmployeeDTO
    {

        [Required(ErrorMessage = "Name is required.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Enter a string between 3 and 50.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%?&])[A-Za-z\d@$!%?&]{8,}$", ErrorMessage = "Invaild Password")]
        [StringLength(100, ErrorMessage = "Password length must be between 8 and 100 characters.", MinimumLength = 8)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Phone number is required.")]
        [RegularExpression(@"(01)[0125][0-9]{8}$", ErrorMessage = "Phone number must be a vaild phone number")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Department is required.")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Department must be between 3 and 100 characters.")]
        public string Department { get; set; }

        [Required(ErrorMessage = "Date of joining is required.")]
        public DateOnly DateOfJoining { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
