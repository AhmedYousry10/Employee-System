using System.ComponentModel.DataAnnotations;

namespace Employee_Management_System.DTO
{
    public class RegisterDTO
    {
        [Required]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Name must be between 3 and 100 characters.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string Email { get; set; } = "example@smarttechsys.com";

        [Required]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%?&])[A-Za-z\d@$!%?&]{8,}$", ErrorMessage = "Invaild Password")]
        [StringLength(100, ErrorMessage = "Password length must be between 8 and 100 characters.", MinimumLength = 8)]
        public string Password { get; set; } = "123456";

        [Required]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Department must be between 3 and 100 characters.")]
        public string Department { get; set; }

        [Required(ErrorMessage = "Phone number is required.")]
        [RegularExpression(@"(01)[0125][0-9]{8}$", ErrorMessage = "Phone number must be a vaild phone number")]
        public string PhoneNumber { get; set; }

        [Required]
        public DateOnly DateOfJoining { get; set; } = DateOnly.FromDateTime(DateTime.Now);
    }
}
