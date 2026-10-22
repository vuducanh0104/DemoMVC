using DemoMVC.Models;
using Microsoft.EntityFrameworkCore;

namespace DemoMVC.Data
{
    public class ApplicationDbcontext(DbContextOptions<ApplicationDbcontext> options)
        : DbContext(options)
    {
        public DbSet<Person> Person { get; set; } = default!;
        public DbSet<Student> Student { get; set; } = default!;
        public DbSet<Employee> Employees { get; set; } = default!;
        public DbSet<Department> Departments { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Student>()
                .HasOne(student => student.Department)
                .WithMany(department => department.Students)
                .HasForeignKey(student => student.DepartmentCode)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Department>().HasData(
                new Department { DepartmentCode = "CNTT", Name = "Khoa Công nghệ thông tin" },
                new Department { DepartmentCode = "DK", Name = "Khoa Dầu khí" },
                new Department { DepartmentCode = "DC", Name = "Khoa Địa chất" },
                new Department { DepartmentCode = "M", Name = "Khoa Mỏ" },
                new Department { DepartmentCode = "MT", Name = "Khoa Môi trường" },
                new Department { DepartmentCode = "TDB", Name = "Khoa Trắc địa - Bản đồ và Quản lý đất đai" },
                new Department { DepartmentCode = "KT", Name = "Khoa Kinh tế và Quản trị kinh doanh" },
                new Department { DepartmentCode = "XD", Name = "Khoa Xây dựng" },
                new Department { DepartmentCode = "CD", Name = "Khoa Cơ - Điện" },
                new Department { DepartmentCode = "LLCT", Name = "Khoa Lý luận chính trị" },
                new Department { DepartmentCode = "GDQP", Name = "Khoa Giáo dục quốc phòng" },
                new Department { DepartmentCode = "KHCB", Name = "Khoa Khoa học cơ bản" });
        }
    }
}