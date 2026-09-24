using DemoMVC.Models;
using Microsoft.EntityFrameworkCore;

namespace DemoMVC.Data
{
   public class ApplicationDbcontext (DbContextOptions<ApplicationDbcontext> options) : DbContext(options)
    {

        public DbSet<Student> Student { get; set; } = default!;
    }
}