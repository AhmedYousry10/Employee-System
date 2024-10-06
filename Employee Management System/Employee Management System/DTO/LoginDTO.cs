using System.ComponentModel.DataAnnotations;

namespace Employee_Management_System.DTO
{
    public class LoginDTO
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string Email { get; set; } = "example@smarttechsys.com";

        [Required(ErrorMessage = "Password is required")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%?&])[A-Za-z\d@$!%?&]{8,}$", ErrorMessage = "Invaild Password")]
        [StringLength(100, ErrorMessage = "Password length must be between 8 and 100 characters.", MinimumLength = 8)]
        public string Password { get; set; } = "123456";
    }
}
