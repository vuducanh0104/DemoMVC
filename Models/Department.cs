using System.ComponentModel.DataAnnotations;

namespace DemoMVC.Models;

public class Department
{
    [Key]
    [StringLength(10)]
    public string DepartmentCode { get; set; } = string.Empty;

    [Required]
    [StringLength(150)]
    public string Name { get; set; } = string.Empty;

    public ICollection<Student> Students { get; set; } = new List<Student>();
}
