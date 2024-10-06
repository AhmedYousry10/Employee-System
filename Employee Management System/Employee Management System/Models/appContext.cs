using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Employee_Management_System.Models
{
    public class appContext : IdentityDbContext<Employee, Role, Guid>
    {
        public appContext(DbContextOptions<appContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }

    }
}
