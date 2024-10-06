using Microsoft.AspNetCore.Identity;

namespace Employee_Management_System.Models
{
    public class Employee : IdentityUser<Guid>
    {
        public string Name { get; set; }
        public string Department { get; set; }
        public DateOnly DateOfJoining { get; set; }
        public DateOnly? LastLoginDate { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
